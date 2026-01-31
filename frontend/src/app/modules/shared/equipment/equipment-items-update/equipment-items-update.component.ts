import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EquipmentItemsGetAllService } from '../../../../endpoints/equipment-endpoints/equipment-items-gel-all-endpoint.service';
import { EquipmentItemsUpdateService } from '../../../../endpoints/equipment-endpoints/equipment-items-update-endpoint.service';

@Component({
  selector: 'app-equipment-item-update',
  templateUrl: './equipment-items-update.component.html',
  styleUrls: ['./equipment-items-update.component.scss'],
  standalone: false
})
export class EquipmentItemUpdateComponent implements OnInit {

  form!: FormGroup;
  itemId!: number;
  loading = true;

  constructor(
    private fb: FormBuilder,
    private itemGetService: EquipmentItemsGetAllService,
    private itemUpdateService: EquipmentItemsUpdateService,
    private route: ActivatedRoute,
    public router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      serialNumber: ['', Validators.required],
      isAvailable: [true]
    });

    this.itemId = Number(this.route.snapshot.paramMap.get('id'));

    this.loadItem();
  }

  loadItem() {
    this.loading = true;

    this.itemGetService.getItemsByEquipment(0) // ❗ ovo se ne koristi – samo placeholder
    .subscribe({
      next: () => {},
      error: () => {}
    });

    // pravi poziv za jedan item:
    fetch(`http://localhost:7000/api/equipment-items/get/${this.itemId}`)
      .then(res => res.json())
      .then(item => {
        this.form.patchValue(item);
        this.loading = false;
      })
      .catch(() => {
        alert("Error loading item");
        this.loading = false;
      });
  }

  save() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.itemUpdateService.updateItem(this.itemId, this.form.value).subscribe({
      next: () => {
        alert("Item updated successfully");
        this.router.navigate([`/equipment-items-list/${this.itemId}}`]);
      },
      error: () => alert("Error updating item")
    });
  }

}