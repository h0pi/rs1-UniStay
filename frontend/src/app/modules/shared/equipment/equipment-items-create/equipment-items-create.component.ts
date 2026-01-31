import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EquipmentItemCreateService } from '../../../../endpoints/equipment-endpoints/equipment-items-create-endpoint.service';

@Component({
  selector: 'app-equipment-item-create',
  templateUrl: './equipment-items-create.component.html',
  styleUrls: ['./equipment-items-create.component.scss'],
  standalone: false
})
export class EquipmentItemCreateComponent implements OnInit {

  form!: FormGroup;
  equipmentId!: number;

  constructor(
    private fb: FormBuilder,
    private create: EquipmentItemCreateService,
    private route: ActivatedRoute,
    public router: Router
  ) {}

  ngOnInit(): void {

    this.equipmentId = Number(this.route.snapshot.paramMap.get('id'));

    this.form = this.fb.group({
      serialNumber: ['', Validators.required],
      //isAvailable: [true],
      equipmentId: [this.equipmentId]
    });
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.create.createItem(this.equipmentId,this.form.value).subscribe({
      next: () => {
        alert('Item created successfully!');
        this.router.navigate([`/equipment-items-list/${this.equipmentId}`]);
      },
      error: () => alert('Error creating item.')
    });
  }
}