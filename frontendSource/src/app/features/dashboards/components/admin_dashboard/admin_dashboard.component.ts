import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { Chart, registerables } from 'chart.js';
Chart.register(...registerables);
@Component({
  selector: 'app-admin_dashboard',
  templateUrl: './admin_dashboard.component.html',
  styleUrls: ['./admin_dashboard.component.css']
})
export class Admin_dashboardComponent implements AfterViewInit {

  @ViewChild('departmentChart') departmentChartRef!: ElementRef;
  @ViewChild('employeeBarChart') employeeBarChartRef!: ElementRef;

  ngAfterViewInit(): void {
    this.initDepartmentChart();
    this.initEmployeeBarChart();
  }

  private initDepartmentChart(): void {
    const ctx = this.departmentChartRef.nativeElement.getContext('2d');
    new Chart(ctx, {
      type: 'doughnut',
      data: {
        labels: ['IT', 'Kinh doanh', 'Tài chính', 'Marketing', 'Nhân sự', 'Hành chính'],
        datasets: [{
          data: [45, 65, 30, 40, 25, 15],
          backgroundColor: [
            '#4361ee',
            '#3a0ca3',
            '#7209b7',
            '#f72585',
            '#4cc9f0',
            '#4895ef'
          ],
          borderWidth: 0
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'right',
            labels: {
              padding: 20,
              usePointStyle: true,
              pointStyle: 'circle',
              font: {
                family: "'Roboto', sans-serif",
                size: 12
              }
            }
          },
          tooltip: {
            backgroundColor: 'rgba(0, 0, 0, 0.7)',
            padding: 12,
            titleFont: {
              size: 14,
              family: "'Roboto', sans-serif"
            },
            bodyFont: {
              size: 13,
              family: "'Roboto', sans-serif"
            },
            callbacks: {
              label: function(context) {
                return `${context.label}: ${context.parsed} nhân viên (${Math.round(context.parsed / 220 * 100)}%)`;
              }
            }
          }
        },
        cutout: '70%'
      }
    });
  }

  private initEmployeeBarChart(): void {
    const ctx = this.employeeBarChartRef.nativeElement.getContext('2d');
    const months = ['Tháng 12', 'Tháng 1', 'Tháng 2', 'Tháng 3', 'Tháng 4', 'Tháng 5'];
    
    new Chart(ctx, {
      type: 'bar',
      data: {
        labels: months,
        datasets: [
          {
            label: 'Tuyển mới',
            data: [12, 15, 10, 8, 13, 18],
            backgroundColor: '#4cc9f0',
            borderRadius: 5,
            barPercentage: 0.6,
            categoryPercentage: 0.8
          },
          {
            label: 'Nghỉ việc',
            data: [8, 10, 7, 9, 8, 6],
            backgroundColor: '#f72585',
            borderRadius: 5,
            barPercentage: 0.6,
            categoryPercentage: 0.8
          },
          {
            label: 'Tổng biến động',
            data: [4, 5, 3, -1, 5, 12],
            backgroundColor: '#4361ee',
            borderRadius: 5,
            barPercentage: 0.6,
            categoryPercentage: 0.8
          }
        ]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: {
            position: 'top',
            align: 'end',
            labels: {
              usePointStyle: true,
              pointStyle: 'circle',
              padding: 20,
              font: {
                family: "'Roboto', sans-serif",
                size: 12
              }
            }
          },
          tooltip: {
            backgroundColor: 'rgba(0, 0, 0, 0.7)',
            padding: 12,
            titleFont: {
              size: 14,
              family: "'Roboto', sans-serif"
            },
            bodyFont: {
              size: 13,
              family: "'Roboto', sans-serif"
            }
          }
        },
        scales: {
          x: {
            grid: {
              display: false
            },
            ticks: {
              font: {
                family: "'Roboto', sans-serif",
                size: 12
              }
            }
          },
          y: {
            beginAtZero: true,
            grid: {
              color: 'rgba(0, 0, 0, 0.05)'
            },
            ticks: {
              font: {
                family: "'Roboto', sans-serif",
                size: 12
              },
              callback: function(value) {
                return value + ' người';
              }
            }
          }
        }
      }
    });
  }
}