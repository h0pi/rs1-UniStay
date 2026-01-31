import { Component, OnInit } from '@angular/core';
import { FaultGetAllEndpointService } from '../../../../endpoints/fault-endpoints/fault-get-all-endpoint.service';
import { FaultDeleteEndpointService } from '../../../../endpoints/fault-endpoints/fault-delete-endpoint.service';
import { Router } from '@angular/router';
import { FaultUpdateEndpointService } from '../../../../endpoints/fault-endpoints/fault-update-endpoint.service';

@Component({
  selector: 'app-fault-list',
  templateUrl: './fault-list.component.html',
  styleUrls: ['./fault-list.component.scss'],
  standalone:false
})
export class FaultListComponent implements OnInit {
  faults: any[] = [];
  filteredFaults: any[] = [];

  filters = {
    title: '',
    reportedByUserID: '',
    isResolved: '',
    from: '',
    to:''
  };

  loading = false;

  constructor(
    private faultGetAllService: FaultGetAllEndpointService,
    private faultDeleteService: FaultDeleteEndpointService,
    private faultUpdateService:FaultUpdateEndpointService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadFaults();
  }

  loadFaults(): void {
    this.loading = true;
    this.faultGetAllService.getAllFaults().subscribe({
      next: (res:any) => {
        this.faults = res;
        this.filteredFaults = res;
        this.loading = false;
      },
      error: () => {
        alert('Error loading faults');
        this.loading = false;
      }
    });
  }

applyFilters() {
    this.filteredFaults = this.faults.filter(f => {
      const matchesTitle =
        !this.filters.title ||
        f.title.toLowerCase().includes(this.filters.title.toLowerCase());

      const matchesReportedBy =
        !this.filters.reportedByUserID ||
        f.reportedByUserID == Number(this.filters.reportedByUserID);

      const matchesIsResolved =
        this.filters.isResolved === '' ||
        String(f.isResolved) === this.filters.isResolved;

      const matchesFrom =
        !this.filters.from ||
        new Date(f.reportedAt) >= new Date(this.filters.from);

      const matchesTo =
        !this.filters.to ||
        new Date(f.reportedAt) <= new Date(this.filters.to);

      return matchesTitle && matchesReportedBy && matchesIsResolved && matchesFrom && matchesTo;
    });
  }

  clearFilters(): void {
    this.filters = { title: '', reportedByUserID: '', isResolved: '', from: '', to: '' };
    this.filteredFaults = this.faults;
  }

  deleteFault(id: number): void {
    console.log("ID I am sending: ",id);
    if (confirm('Are you sure yout wand to delete this fault?')) {
      this.faultDeleteService.deleteFault(id).subscribe({
        next: () => {
          alert('Fault deleted');
          this.loadFaults();
        },
        error: (err) => {
          console.error("Backend error: ",err);
          alert('Error deleting');}
      });
    }
  }

  updateFault(id: number): void {
    this.router.navigate(['fault-update', id]);
  }

  
}