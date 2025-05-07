import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { Chart, registerables } from 'chart.js';
Chart.register(...registerables);
@Component({
  selector: 'app-admin_dashboard',
  templateUrl: './admin_dashboard.component.html',
  styleUrls: ['./admin_dashboard.component.css']
})
export class Admin_dashboardComponent implements AfterViewInit  {

  @ViewChild('departmentChart') departmentChartRef!: ElementRef;

  ngAfterViewInit(): void {
    const ctx = this.departmentChartRef.nativeElement.getContext('2d');
    new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: ['IT', 'Kinh doanh', 'Tài chính', 'Marketing', 'Nhân sự', 'Hành chính'],
        datasets: [{
          data: [45, 65, 30, 40, 25, 15],
          backgroundColor: [
            '#1a73e8',
            '#4285f4',
            '#0d47a1',
            '#4fc3f7',
            '#0f9d58',
            '#f4b400'
          ],
          borderWidth: 0
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'bottom',
            labels: {
              padding: 20,
              usePointStyle: true,
              pointStyle: 'circle'
            }
          }
        },
        cutout: '70%'
      }
    });
  }

}
