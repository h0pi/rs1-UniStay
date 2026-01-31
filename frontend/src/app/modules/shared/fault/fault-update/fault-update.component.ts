import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FaultGetByIdEndpointService } from '../../../../endpoints/fault-endpoints/fault-get-by-id-endpoint.service';
import { FaultUpdateEndpointService } from '../../../../endpoints/fault-endpoints/fault-update-endpoint.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { FaultCreateEndpointService } from '../../../../endpoints/fault-endpoints/fault-create-endpoint.service';

@Component({
  selector: 'app-fault-update',
  templateUrl: './fault-update.component.html',
  standalone:false
})
export class FaultUpdateComponent implements OnInit {
  form!:FormGroup;
  id:number | null=null;
  isEdit=false;
  loading = false;

  constructor(
    private fb:FormBuilder,
    private route: ActivatedRoute,
    private faultGetByIdService: FaultGetByIdEndpointService,
    private faultUpdateService: FaultUpdateEndpointService,
    private faultCreateService:FaultCreateEndpointService,
    private router: Router
  ) {}

 ngOnInit(): void {
    this.form = this.fb.group({
      title: ['', Validators.required],
      description: [''],
      priority: [''],
      isResolved: [false],
      status:[''],
      resolvedAt:['']
    });

    const idParam = this.route.snapshot.paramMap.get('id');
    this.id = idParam ? Number(idParam) : null;
    this.isEdit = !!this.id;

    if (this.isEdit && this.id) {
      this.loading = true;
      this.faultGetByIdService.getFaultById(this.id).subscribe({
        next: (data) => {
          // map server fields to form
          this.form.patchValue({
            title: data.title,
            description: data.description,
            priority: data.priority,
            isResolved: data.isResolved,
            resolvedAt: data.resolvedAt ? data.resolvedAt.split?.('T')[0] ?? data.resolvedAt : null,
            status:data.status
          });
          this.loading = false;
        },
        error: (err) => {
          console.error('Error loading fault', err);
          alert('Greška pri učitavanju');
          this.loading = false;
          this.router.navigate(['../fault-list'], { relativeTo: this.route.parent });
        }
      });
    }
  }

  saveFault() {
    if(this.form.invalid){
      this.form.markAllAsTouched();
      return;
    }

    const payload = this.form.value;
    if (this.isEdit && this.id) {
      this.faultUpdateService.updateFault(this.id, payload).subscribe({
        next: () => {
          alert('Uspješno ažurirano');
          this.router.navigate(['../fault-list'], { relativeTo: this.route.parent });
        },
        error: (err) => {
          console.error(err);
          alert('Greška pri spremanju');
        }
      });
    } else {
      this.faultCreateService.createFault(payload).subscribe({
        next: () => {
          alert('Kvar dodat');
          this.router.navigate(['../fault-list'], { relativeTo: this.route.parent });
        },
        error: (err) => {
          console.error(err);
          alert('Greška pri dodavanju');
        }
      });
  }
}

  back() {
    this.router.navigate(['fault-list']);
  }
}