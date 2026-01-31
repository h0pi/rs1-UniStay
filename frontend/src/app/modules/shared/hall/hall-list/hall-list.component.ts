import { Component, OnInit } from '@angular/core';
import { HallGetAllEndpointService } from '../../../../endpoints/hall-endpoints/hall-get-all-endpoint.service'
import { HallDeleteEndpointService } from '../../../../endpoints/hall-endpoints/hall-delete-endpoint.service';
import { Router } from '@angular/router';
import { HallUpdateEndpointService } from '../../../../endpoints/hall-endpoints/hall-update-endpoint.service';



@Component({
  selector: 'app-hall-list',
  templateUrl: './hall-list.component.html',
  styleUrls: ['./hall-list.component.scss'],
  standalone:false
})
export class HallListComponent implements OnInit {
  halls: any[] = [];
  filteredHalls: any[] = [];

  filters = {
    name: '',
    capacity: '',
    availableFrom: '',
    availableTo: '',
    isAvailable: ''
  };

  loading = false;

  constructor(
    private hallGetAllService: HallGetAllEndpointService,
    private hallDeleteService: HallDeleteEndpointService,
    private hallUpdateService:HallUpdateEndpointService,
    private router: Router
  ) {}

  ngOnInit(): void {
    console.log('HallListComponent ngOnInit');
    this.loadHalls();
  }

  loadHalls(): void {
    this.loading = true;
    this.hallGetAllService.getAllHalls().subscribe({
      next: (res:any) => {
        this.halls = res;
        this.filteredHalls = res;
        this.loading = false;
      },
      error: () => {
        alert('Error loading halls');
        this.loading = false;
      }
    });
  }

  applyFilters(): void {
    this.filteredHalls = this.halls.filter((hall) => {
      const matchesName = hall.name.toLowerCase().includes(this.filters.name.toLowerCase());
      const matchesCapacity = !this.filters.capacity || hall.capacity >= +this.filters.capacity;
      const matchesAvailable = this.filters.isAvailable === '' || hall.isAvailable === (this.filters.isAvailable === 'true');
      const matchesDateFrom = !this.filters.availableFrom || new Date(hall.availableFrom) >= new Date(this.filters.availableFrom);
      const matchesDateTo = !this.filters.availableTo || new Date(hall.availableTo) <= new Date(this.filters.availableTo);

      return matchesName && matchesCapacity && matchesAvailable && matchesDateFrom && matchesDateTo;
    });
  }

  clearFilters(): void {
    this.filters = { name: '', capacity: '', availableFrom: '', availableTo: '', isAvailable: '' };
    this.filteredHalls = this.halls;
  }

  deleteHall(id: number): void {
    console.log("ID I am sending: ",id);
    if (confirm('Are you sure yout wand to delete this hall?')) {
      this.hallDeleteService.deleteHall(id).subscribe({
        next: () => {
          alert('Hall deleted');
          this.loadHalls();
        },
        error: (err) => {
          console.error("Backend error: ",err);
          alert('Error deleting');}
      });
    }
  }

  updateHall(id: number): void {
    this.router.navigate(['update', id]);
  }

  
}