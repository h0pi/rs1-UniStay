import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { HallGetByIdEndpointService } from '../../../../endpoints/hall-endpoints/hall-get-by-id-endpoint.service';
import { HallUpdateEndpointService } from '../../../../endpoints/hall-endpoints/hall-update-endpoint.service';
import { HallCreateEndpointService } from '../../../../endpoints/hall-endpoints/hall-create-endpoint.service';
import { HallGetAllEndpointService } from '../../../../endpoints/hall-endpoints/hall-get-all-endpoint.service';
@Component({
  selector: 'app-hall-add',
  templateUrl: './hall-add.component.html',
  styleUrls: ['./hall-add.component.scss'],
  standalone:false
})
export class HallAddComponent implements OnInit {
  form!: FormGroup;
  hallId!: number | null;
  isAddMode = false;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private hallGetByIdService: HallGetByIdEndpointService,
    private hallUpdateService: HallUpdateEndpointService,
    private hallCreateService: HallCreateEndpointService,
    private hallGetAllService:HallGetAllEndpointService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', Validators.required],
      capacity: ['', [Validators.required, Validators.min(1)]],
      description: ['', [Validators.required, Validators.minLength(10)]],
      availableFrom: ['', Validators.required],
      availableTo: ['', Validators.required],
      isAvailable: [true]
    });

    this.hallId = Number(this.route.snapshot.paramMap.get('id'));
    this.isAddMode = !!this.hallId;

    if (this.isAddMode) {
      this.loadHall();
    }
  }

  loadHall(): void {
    this.loading = true;
    this.hallGetByIdService.getHallById(this.hallId!).subscribe({
      next: (hall) => {
        this.form.patchValue(hall);
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        alert('Error loading hall');
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const hallData = this.form.value;

    this.loading = true;

    if (this.isAddMode) {
      this.hallUpdateService.updateHall(this.hallId!, hallData).subscribe({
        next: () => {
          alert('Hall successfully updated!');
          this.router.navigate(['/admin/hall']); // ili /employee/hall
        },
        error: (err) => {
          console.error(err);
          alert('Error updating hall');
          this.loading = false;
        }
      });
    } else {
      this.hallCreateService.createHall(hallData).subscribe({
        next: () => {
          alert('Hall successfully added!');
          this.router.navigate(['/admin/hall']);
        },
        error: (err) => {
          console.error(err);
          alert('Error adding hall');
          this.loading = false;
        }
      });
    }
  }
}