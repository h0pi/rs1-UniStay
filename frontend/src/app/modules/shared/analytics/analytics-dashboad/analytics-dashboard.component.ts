import { Component, OnInit, OnDestroy } from '@angular/core';
import { AnalyticsService } from '../../../../endpoints/analytics/analytics.service';

@Component({
  selector: 'app-analytics-dashboard',
  templateUrl: './analytics-dashboard.component.html',
  styleUrls: ['./analytics-dashboard.component.scss'],
  standalone:false
})
export class AnalyticsDashboardComponent implements OnInit, OnDestroy {

  analytics={
    activeUsers:0,
    totalUsers:0,
    totalMessages:0

  }

  constructor(private analyticsService: AnalyticsService) {}

ngOnInit(): void {
  const userId = Number(localStorage.getItem('id'));

  if (!userId) {
    return;
  }

  this.analyticsService.startConnection(userId);

  this.analyticsService.analytics$
    .subscribe(data => {
      this.analytics = data;
    });
}



  ngOnDestroy():void{
    this.analyticsService.stopConnection();
  }
}