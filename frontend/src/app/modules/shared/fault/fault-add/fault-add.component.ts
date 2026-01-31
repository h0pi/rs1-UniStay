import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FaultGetByIdEndpointService } from '../../../../endpoints/fault-endpoints/fault-get-by-id-endpoint.service';
import { FaultUpdateEndpointService } from '../../../../endpoints/fault-endpoints/fault-update-endpoint.service';
import { FaultCreateEndpointService, FaultCreateRequest } from '../../../../endpoints/fault-endpoints/fault-create-endpoint.service';
import { FaultGetAllEndpointService } from '../../../../endpoints/fault-endpoints/fault-get-all-endpoint.service';
import { RoomGetAllEndpointService } from '../../../../endpoints/fault-endpoints/room-get-all-endpoint.service';

@Component({
  selector: 'app-fault-add',
  templateUrl: './fault-add.component.html',
    styleUrls: ['./fault-add.component.scss'],
  standalone:false
})
export class FaultAddComponent {
  form!: FormGroup;
  faultId!: number | null;
  isAddMode = false;
  loading = false;
  rooms:any[]=[];

  constructor(
    private fb: FormBuilder,
    private faultGetByIdService: FaultGetByIdEndpointService,
    private faultUpdateService: FaultUpdateEndpointService,
    private faultCreateService: FaultCreateEndpointService,
    private faultGetAllService:FaultGetAllEndpointService,
    private roomGetAllService:RoomGetAllEndpointService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      title:['',Validators.required],
      description:['',Validators.required],
      reportedBy:[''],
      roomID:['',Validators.required]
    });

    this.faultId = Number(this.route.snapshot.paramMap.get('id'));
    this.isAddMode = !!this.faultId;

    const userId=Number(localStorage.getItem("id"));
    this.form.patchValue({reportedBy:userId});

    this.roomGetAllService.getAllRooms().subscribe({
      next:(data)=>this.rooms=data
    });

    this.form.patchValue({reportedBy:userId});

    if (this.isAddMode) {
      this.loadFault();
    }
  }

  loadFault(): void {
    this.loading = true;
    this.faultGetByIdService.getFaultById(this.faultId!).subscribe({
      next: (fault) => {
        this.form.patchValue(fault);

        if(fault.resolvedAt!=null){
  this.form.patchValue({isResolved:true});
}

        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        alert('Error loading fault');
        this.loading = false;
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }



const payload: FaultCreateRequest = {
  title: this.form.value.title,
  description: this.form.value.description,
  reportedBy: this.form.value.reportedBy,
  roomID:this.form.value.roomID
  
};

    this.loading = true;

    if (this.isAddMode) {
      this.faultUpdateService.updateFault(this.faultId!, payload).subscribe({
        next: () => {
          alert('Fault successfully updated!');
          this.router.navigate(['/admin/fault']); 
        },
        error: (err) => {
          console.error(err);
          alert('Error updating fault');
          this.loading = false;
        }
      });
    } else {
      this.faultCreateService.createFault(payload).subscribe({
        next: () => {
          alert('Fault successfully added!');
          this.router.navigate(['/admin/fault']);
        },
        error: (err) => {
          console.error(err);
          alert('Error adding fault');
          this.loading = false;
        }
      });
    }
  }
}