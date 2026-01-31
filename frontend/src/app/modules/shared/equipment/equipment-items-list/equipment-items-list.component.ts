import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { EquipmentItemsGetAllService } from '../../../../endpoints/equipment-endpoints/equipment-items-gel-all-endpoint.service';
import { EquipmentItemsUpdateService } from '../../../../endpoints/equipment-endpoints/equipment-items-update-endpoint.service';
import { EquipmentGetOneService } from '../../../../endpoints/equipment-endpoints/equipment-items-get-one-endpoint.service';

@Component({
  selector: 'app-equipment-items-list',
  templateUrl: './equipment-items-list.component.html',
  styleUrls: ['./equipment-items-list.component.scss'],
  standalone:false
})
export class EquipmentItemsListComponent implements OnInit {

  equipmentId!: number;
  items: any[] = [];
  loading = false;
  equipmentName:string='';
  

  constructor(
    private route: ActivatedRoute,
    private get: EquipmentItemsGetAllService,
    private update: EquipmentItemsUpdateService,
    private router:Router,
    private getone:EquipmentGetOneService

  ) {}

  ngOnInit(): void {
    this.equipmentId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadItems();

    this.equipmentId = Number(this.route.snapshot.paramMap.get('id'));

this.getone.getOne(this.equipmentId).subscribe(eq => {
  this.equipmentName = eq.name;
});

this.loadItems();
  }

  loadItems() {
    this.loading = true;
    this.get.getItemsByEquipment(this.equipmentId).subscribe({
      next: get => {
        this.items = get;
        this.loading = false;
      },
      error: () => alert("Error loading equipment items")
    });
  }

  toggleAvailability(item: any) {
    const newStatus = !item.isAvailable;

    this.update.updateAvailability(item.recordID, newStatus).subscribe({
      next: () => this.loadItems(),
      error: () => alert("Error updating availability")
    });
  }

  addNewItem(){
    this.router.navigate([`/equipment-items-create/${this.equipmentId}`]);

  }

  showModal = false;
modalData: any = {
  itemId: 0,
  assignedAt: '',
  returnedAt: '',
  location: ''
};

openAssignModal(item: any) {
  this.modalData = {
    recordID: item.recordID,
    assignedAt: '',
    returnedAt: '',
    location: ''
  };
  this.showModal = true;
}

closeModal() {
  this.showModal = false;
}

confirmAssign() {
  const payload = {
    equipmentRecordID: this.modalData.recordID,
    assignedAt: this.modalData.assignedAt ? new Date(this.modalData.assignedAt).toISOString() : null,
    returnedAt: this.modalData.returnedAt ? new Date(this.modalData.returnedAt).toISOString() : null,
    location: this.modalData.location,
  };

  console.log("ASSIGN PAYLOAD:", payload);

  this.update.assignItem(payload).subscribe({
    next: () => {
      this.closeModal();
      this.loadItems();
    },
    error: (err) => {
      console.error('Assign error', err);
      alert('Error assigning item');
    }
  });
}

releaseItem(item: any) {
  if (!confirm("Are you sure you want to release this item?")) return;

  this.update.releaseItem(item.recordID).subscribe({
    next: () => this.loadItems(),
    error: () => alert("Error releasing item")
  });
}

delete(id:number) {
  if (!confirm("Delete?")) return;

  this.update.deleteRecord(id).subscribe({
    next: () => this.loadItems(),
    error: () => alert("Error deleting item")
  });
}
}