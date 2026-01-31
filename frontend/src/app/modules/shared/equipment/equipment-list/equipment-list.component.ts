import { Component, OnInit } from '@angular/core';
import { EquipmentGetAllEndpointService } from '../../../../endpoints/equipment-endpoints/equipment-get-all-endpoint.service';
import { EquipmentDeleteEndpointService } from '../../../../endpoints/equipment-endpoints/equipment-delete-endpoint.service';
import { Router } from '@angular/router';
import { EquipmentUpdateComponent } from '../equipment-update/equipment-update.component';


@Component({
  selector: 'app-equipment-list',
  templateUrl: './equipment-list.component.html',
  styleUrls: ['./equipment-list.component.scss'],
  standalone:false
})
export class EquipmentListComponent implements OnInit {
  equipments: any[] = [];
  filteredEquipment: any[] = [];
  filters = { name: '', type: ''};
  loading = false;

  constructor(
    private equipmentGetAllservice: EquipmentGetAllEndpointService,
    private equipmentDeleteService: EquipmentDeleteEndpointService,
    private router: Router
  ){}

  ngOnInit(): void {
    this.loadEquipment();
  }


  loadEquipment(): void {
    this.loading = true;
    this.equipmentGetAllservice.getAllEquipments().subscribe({
      next:(res:any)=>{
        this.equipments=res;
        this.filteredEquipment=res;
        this.loading=false;
      },
      error:()=>{
        alert('Error loading equipments');
        this.loading=false;
      }
    });
  }

 applyFilters() {
  const name = (this.filters.name || '').toLowerCase();
  const type = (this.filters.type || '').toLowerCase();

  this.filteredEquipment = this.equipments.filter(e => {
    const itemName = (e.name || '').toLowerCase();
    const itemType = (e.equipmentType || '').toLowerCase();

    return itemName.includes(name) && itemType.includes(type);
  });
}

  clearFilters():void{ 
    this.filters = { name:'', type:'' }; 
    this.filteredEquipment=this.equipments;
  }

  deleteEquipment(id:number):void{
if (confirm('Are you sure yout wand to delete this equipment?')) {
      this.equipmentDeleteService.deleteEquipment(id).subscribe({
        next: () => {
          alert('Equipment deleted');
          this.loadEquipment();
        },
        error: (err) => {
          console.error("Backend error: ",err);
          alert('Error deleting');}
      });
    }}

  updateEquipment(id:number):void{ 
    this.router.navigate([`equipment-update/${id}`]); 
  }

     openItems(id:number){
    this.router.navigate([`equipment-items-list/${id}`]);
  }
}