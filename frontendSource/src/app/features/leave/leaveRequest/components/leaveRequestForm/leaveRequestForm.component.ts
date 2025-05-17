import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, Observable, of } from 'rxjs';
import { map, startWith, switchMap } from 'rxjs/operators';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';

import { LeaveRequestService } from '../../leaveRequest.service';
import { LeaveRequest, LeaveBalanceResSub, LeaveRequestDetailsRes, LeaveRequestDetaislDto, LeaveRequestDto, LeaveRequestRes, Dataset_Leave, DataResponse_Leave } from '../../entity';

@Component({
  selector: 'app-leaveRequestForm',
  templateUrl: './leaveRequestForm.component.html',
  styleUrls: ['./leaveRequestForm.component.css']
})
export class LeaveRequestFormComponent implements OnInit {

// lookup
  defaultCode: string;
  statusList: any[] = [];
  employeeList: any[] = [];
  leaveTypeList: any[] = [];
  filteredEmployees: Observable<any[]>;
  balanceLeave: LeaveBalanceResSub[] = [];

  inforsub : any[] = [];
  

  leaveRequestId: number;
  isEditMode: boolean = false;
  LeaveRequestForm: FormGroup;
  LeaveRequestDetais: LeaveRequestDetaislDto[] = [];
  
  // For modal
  displayLeaveRequestDetaisModal: boolean = false;
  LeaveRequestDetaisForm: FormGroup;
  editingLeaveRequestDetaisId: number | null = null;
  
  activeTab: number = 0;
  


  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private LeaveRequestService: LeaveRequestService,
    private lookupService: LookupService
  ) { this.initEmptyForms(); }

  ngOnInit(): void {
    // 1. Đầu tiên, tạo forms trống
    // 2. Sau đó tải dữ liệu dropdown
    // 3. Cuối cùng khởi tạo form với giá trị mặc định nếu cần
    
    // Lắng nghe tham số route
    this.route.params.subscribe(params => {
      this.leaveRequestId = +params['id'];
      if (this.leaveRequestId) {
        this.isEditMode = true;
      }
    });
    
    // Tải dữ liệu dropdown
    this.loadDropdownData();
  }

  // Khởi tạo form trống ngay từ đầu để tránh lỗi null
  initEmptyForms(): void {
    this.LeaveRequestForm = this.fb.group({
      voucher_code: [null, Validators.required],
      employeeId: [null, Validators.required],
      createDate: ['', Validators.required],
      totalDays: [0, Validators.required],
      approvalStatus: [0, [Validators.required, Validators.min(0)]],
      detail_note: ['', Validators.required]
    });

    this.LeaveRequestDetaisForm = this.fb.group({
      leaveRequestId: ['', Validators.required],
      status: [1, Validators.required],
      reason: ['', Validators.required],
      leaveType: [null, Validators.required],
      changeField: [''],
      totalDays: [0],
      endDate: [null, Validators.required],
      startDate: [null, Validators.required],
      workCode: [1, Validators.required],
    });
  }

  // Getter cho danh sách phụ lục đã lọc (không được đánh dấu để xóa)
  get filteredLeaveRequestDetais(): any[] {
    return this.LeaveRequestDetais.filter(item => item['actionType'] !== 'D');
  }

  initFormsWithData(): void {
    
    this.setupAutoComplete();
    
    if (this.isEditMode) {
      this.loadData();
    }
  }

  setupAutoComplete(): void {

    if(!this.isEditMode){
    this.LeaveRequestForm.patchValue({ voucher_code: this.defaultCode });
    }

  const employeeIdControl = this.LeaveRequestForm.get('employeeId');
  if (employeeIdControl) {
    this.filteredEmployees = employeeIdControl.valueChanges.pipe(
      startWith(''),
      map(value => {
        if (typeof value === 'string') {
          return this._filterEmployees(value);
        } else {
          // If it's already an ID, keep showing all employees
          return this.employeeList.slice();
        }
      })
    );
  }
}

// Improved version of _filterEmployees
private _filterEmployees(value: string): any[] {
  const filterValue = value.toLowerCase();
  return this.employeeList.filter(emp => 
    emp.form_name.toLowerCase().includes(filterValue) || 
    (emp.form_value && emp.form_value.toString().toLowerCase().includes(filterValue))
  );
}

  loadDropdownData(): void {
    forkJoin({
      defaultCode: this.LeaveRequestService.getNew(),
      statusList: this.lookupService.getSysDmtt("LEAVE"),
      employeeList: this.lookupService.getEmployees(),
      leaveTypeList: this.lookupService.getLeaveTypes()
    }).subscribe({
      next: (result) => {
        this.statusList = result.statusList;
        this.employeeList = result.employeeList;
        this.defaultCode = result.defaultCode.data.default_phCode;
        this.balanceLeave = result.defaultCode.data.balanceLeave;
        this.leaveTypeList = result.leaveTypeList;
        
        // Khởi tạo lại form và thiết lập autocomplete sau khi có dữ liệu
        this.initFormsWithData();
      },
      error: (error) => {
        console.error('Lỗi khi tải dữ liệu dropdown:', error);
      }
    });
  }

  // Phương thức lấy tên nhân viên từ ID
  getEmployeeName(id: string | null): string {
    if (!id) return '';
    const employee = this.employeeList.find(emp => emp.id === id);
    return employee ? employee.form_name : '';
  }

  // Lọc nhân viên dựa trên đầu vào
 

  // Phương thức hiển thị tên nhân viên trong trường nhập
 displayFn = (id: string | number | null): string => {
  if (id === null || id === undefined) return '';
  
  // Convert to string for comparison if needed
  const employeeId = id.toString();
  const emp = this.employeeList.find(e => e.id.toString() === employeeId);
  return emp ? emp.form_value : '';
};

  // Phương thức đảm bảo chỉ có thể chọn nhân viên hợp lệ
  validateSelection(): void {
  const employeeIdControl = this.LeaveRequestForm.get('employeeId');
  if (!employeeIdControl) return;
  
  const currentValue = employeeIdControl.value;
  
  // If it's a string (user typing), find matching employee
  if (typeof currentValue === 'string') {
    const matchingEmployee = this.employeeList.find(
      emp => emp.id.toLowerCase() === currentValue.toLowerCase()
    );
    
    if (!matchingEmployee) {
      employeeIdControl.setValue(null);
    } else {
      employeeIdControl.setValue(matchingEmployee.id);
    }
  } 
  // If it's not a string but also not a valid ID in our list, reset it
  else if (currentValue !== null) {
    const exists = this.employeeList.some(emp => emp.id === currentValue);
    if (!exists) {
      employeeIdControl.setValue(null);
    }
  }
}

  loadData(): void {
    this.LeaveRequestService.getById(this.leaveRequestId).subscribe({
      next: (response: any) => {

        if (response.success) {
          console.log(response)
          const ph = response.data.ph;
          this.LeaveRequestForm.patchValue({
            employeeId: ph.employeeId,
            createDate: ph.createDate ? this.formatDateForInput(ph.createDate) : null,
            totalDays: ph.totalDays,
            voucher_code: ph.voucher_code,
            detail_note: ph.detail_note,
            approvalStatus: ph.approvalStatus,
          });
          setTimeout(() => {
          this.LeaveRequestForm.get('employeeId')?.setValue(ph.employeeId);
        });
          if (response.data.ct) {
            this.LeaveRequestDetais = Array.isArray(response.data.ct) 
              ? response.data.ct 
              : [response.data.ct];
            
            // Format dates for addendums
            this.LeaveRequestDetais.forEach(item => {
              if (item.startDate) {
                item.startDate = new Date(item.startDate);
              }
              if (item.endDate) {
                item.endDate = new Date(item.endDate);
              }
              // Initialize actionType if not present
              if (!item['actionType']) {
                item['actionType'] = 'N'; // N for No changes (original data)
              }
            });
          }
          
          
        } else {
          this.showErrorMessage(response.message || 'Failed to load contract data');
        }
      },
      error: (error) => {
        this.showErrorMessage('Failed to load contract data');
        console.error(error);
      }
    });
  }

  // Helper method to format dates for HTML date inputs (YYYY-MM-DD)
  formatDateForInput(dateString: string | Date): string {
    const date = new Date(dateString);
    return date.toISOString().split('T')[0];
  }

  saveLeaveRequest(): void {
    if (this.LeaveRequestForm.invalid) {
      // Mark all fields as touched to show validation errors
      Object.keys(this.LeaveRequestForm.controls).forEach(key => {
        const control = this.LeaveRequestForm.get(key);
        if (control) {
          control.markAsTouched();
        }
      });
      this.showErrorMessage('Please correct the errors in the form');
      return;
    }

    const leaveRequestData: Dataset_Leave = {
      ph: {
        ...this.LeaveRequestForm.value
      },
      ct: this.LeaveRequestDetais.map(item => ({
        id: 0,
        leaveRequestId: 0,
        startDate: item.startDate,
        endDate: item.endDate,
        reason: item.reason,
        leaveType: item.leaveType,
        totalDays: item.totalDays,
        workCode: item.workCode,
        status: 1,
        actionType: item['actionType'] || 'A' // Default to Add if not specified
      }))
    };

    if (this.isEditMode) {
      this.LeaveRequestService.update(this.leaveRequestId, leaveRequestData).subscribe({
        next: (response: any) => {
          if (response.success) {
            this.showSuccessMessage('leaverequest updated successfully');
            this.router.navigate(['/leave-requests']);
          } else {
            this.showErrorMessage(response.message || 'Failed to update leave-requests');
          }
        },
        error: (error) => {
          this.showErrorMessage('Failed to update leave-requests');
          console.error(error);
        }
      });
    } else {
      this.LeaveRequestService.create(leaveRequestData).subscribe({
        next: (response: any) => {
          if (response.success) {
            this.showSuccessMessage('leaverequest created successfully');
            this.router.navigate(['/leave-requests']);
          } else {
            this.showErrorMessage(response.message || 'Failed to create leave-requests');
          }
        },
        error: (error) => {
          this.showErrorMessage('Failed to create leave-requests');
          console.error(error);
        }
      });
    }
  }

  // Show messages - you can implement these with your preferred notification library
  showSuccessMessage(message: string): void {
    // Use any notification library like ngx-toastr or implement a custom solution
    alert(message); // Fallback to alert for this example
  }

  showErrorMessage(message: string): void {
    // Use any notification library like ngx-toastr or implement a custom solution
    alert(message); // Fallback to alert for this example
  }

  // Addendum related methods

  
 

  editLeaveReqestDetail(item: LeaveRequestDetaislDto): void {
    this.openDetailModal(item);
  }

  deleteAddendum(item: LeaveRequestDetaislDto): void {
    if (confirm('Bạn có chắc chắn muốn xóa?')) {
      if (item.id > 0) {
        // Existing addendum - mark for deletion
        item['actionType'] = 'D'; 
      } else {
        // New addendum - remove from array
        this.LeaveRequestDetais = this.LeaveRequestDetais.filter(a => a.id !== item.id);
      }
    }
  }

  cancel(): void {
    this.router.navigate(['/leave-requests']);
  }

  changeTab(index: number): void {
    this.activeTab = index;
  }


// Update the openAddendumModal method to handle the changeField
openDetailModal(item?: LeaveRequestDetaislDto): void {
  // Reset the form first
  this.LeaveRequestDetaisForm.reset({
    status : 1,
    reason : '',
    leaveType : '',
    totalDays: 0,
    endDate: null,
    startDate: null,
    workCode: 0,
    leaveRequestId: null
  });
  
  if (item) {
    this.editingLeaveRequestDetaisId = item.id;
    
    // Set basic values
    this.LeaveRequestDetaisForm.patchValue({
      status : 1,
      reason : item.reason,
      leaveType : item.leaveType,
      totalDays: item.totalDays,
      endDate: item.endDate ? this.formatDateForInput(item.endDate) : null,
      startDate: item.startDate? this.formatDateForInput(item.startDate) : null,
      workCode: item.workCode,
      leaveRequestId: item.leaveRequestId
    });
    
    
  } else {
    this.editingLeaveRequestDetaisId = null;
  }
  
  this.displayLeaveRequestDetaisModal = true;
}

// Also update the saveAddendum method to properly handle the data
saveAddendum(): void {
  if (this.LeaveRequestDetaisForm.invalid) {
    Object.keys(this.LeaveRequestDetaisForm.controls).forEach(key => {
      const control = this.LeaveRequestDetaisForm.get(key);
      if (control) {
        control.markAsTouched();
      }
    });
    return;
  }

  // Get change field value
  
  // Build addendum data with correct field translated for display
  
  
  const DetailsData = {
    ...this.LeaveRequestDetaisForm.value,
  };

  if (this.editingLeaveRequestDetaisId) {
    // Update existing addendum
    const index = this.LeaveRequestDetais.findIndex(a => a.id === this.editingLeaveRequestDetaisId);
    if (index !== -1) {
      // Set action type to 'E' for Edit/Update if it's an existing record (positive ID)
      const actionType = this.editingLeaveRequestDetaisId > 0 ? 'E' : 'A';
      
      this.LeaveRequestDetais[index] = {
        ...this.LeaveRequestDetais[index],
        ...DetailsData,
        id: this.editingLeaveRequestDetaisId,
        leaveRequestId: this.leaveRequestId,
        actionType: actionType
      };
    }
  } else {
    // Add new addendum with a temporary ID (negative to distinguish from server IDs)
    const tempId = -1 * (this.LeaveRequestDetais.length + 1);
    this.LeaveRequestDetais.push({
      ...DetailsData,
      id: tempId,
      leaveRequestId: this.leaveRequestId || 0,
      actionType: 'A'
    } as LeaveRequestDetaislDto);
  }

  this.displayLeaveRequestDetaisModal = false;
  this.editingLeaveRequestDetaisId = null;
}
  
}