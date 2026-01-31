import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EquipmentCreateEndpointService } from '../../../../endpoints/equipment-endpoints/equipment-create-endpoint.service';
import { EquipmentUpdateEndpointService } from '../../../../endpoints/equipment-endpoints/equipment-update-endpoint.service';
import { EquipmentGetByIdEndpointService } from '../../../../endpoints/equipment-endpoints/equpiment-get-by-id-endpoint.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({ selector:'app-equipment-add', 
  templateUrl:'./equipment-add.component.html',
standalone:false
 })
export class EquipmentAddComponent implements OnInit {
  form!: FormGroup;
  id: number | null = null;
  isEdit = false;
  loading = false;

  constructor(private fb: FormBuilder,
              private createSvc: EquipmentCreateEndpointService,
              private updateSvc: EquipmentUpdateEndpointService,
              private getById: EquipmentGetByIdEndpointService,
              private route: ActivatedRoute,
              private router: Router) {}

  ngOnInit(){
    this.form = this.fb.group({
      name: ['', Validators.required],
      description: [''],
      quantity: [1, [Validators.required, Validators.min(0)]],
      availableQuantity: [1, [Validators.required, Validators.min(0)]],
      rentalPrice: [''],
      equipmentType: ['']
    });

    this.id = Number(this.route.snapshot.paramMap.get('id'));
    this.isEdit = !!this.id;
    if (this.isEdit) this.load();
  }

  load(){
    this.getById.getEquipmentById(this.id!).subscribe({
      next: (d:any) => {
        this.form.patchValue(d);
      },
      error: ()=> { alert('Error loading'); this.router.navigate(['../list'], { relativeTo: this.route }); }
    });
  }

  submit(){
    if (this.form.invalid) { this.form.markAllAsTouched(); return; }
    const payload = this.form.value;
    if (this.isEdit){
      this.updateSvc.updateEquipment(this.id!, payload).subscribe({
        next: ()=> { alert('Updated'); this.router.navigate(['../list'], { relativeTo: this.route }); },
        error: ()=> alert('Error updating')
      });
    } else {
      this.createSvc.createEquipment(payload).subscribe({
        next: ()=> { alert('Created'); this.router.navigate(['../list'], { relativeTo: this.route }); },
        error: ()=> alert('Error creating')
      });
    }
  }
}