import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ContractsService } from '../../contracts.service';
import { EmploymentContract, ContractAddendum, ContractAddendumDTO, ContractHistory, ContractDataset } from '../../entity';
import { forkJoin, Observable, of } from 'rxjs';
import { map, startWith, switchMap } from 'rxjs/operators';

import { LookupService } from 'src/app/core/services/lookup/lookup.service';

@Component({
  selector: 'app-contract-form',
  templateUrl: './contractForm.component.html',
  styleUrls: ['./contractForm.component.css']
})
export class ContractFormComponent implements OnInit {

// lookup
  defaultCode: string;
  statusList: any[] = [];
  departmentList: any[] = [];
  positionList: any[] = [];
  contractTypeList: any[] = [];
  employeeList: any[] = [];
  filteredEmployees: Observable<any[]>;


  contractId: number;
  isEditMode: boolean = false;
  contractForm: FormGroup;
  contractAddendums: ContractAddendumDTO[] = [];
  contractHistory: ContractHistory[] = [];
  
  // For modal
  displayAddendumModal: boolean = false;
  addendumForm: FormGroup;
  editingAddendumId: number | null = null;
  
  activeTab: number = 0;
  


  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private contractsService: ContractsService,
    private lookupService: LookupService
  ) { this.initEmptyForms(); }

  ngOnInit(): void {
    // 1. Đầu tiên, tạo forms trống
    // 2. Sau đó tải dữ liệu dropdown
    // 3. Cuối cùng khởi tạo form với giá trị mặc định nếu cần
    
    // Lắng nghe tham số route
    this.route.params.subscribe(params => {
      this.contractId = +params['id'];
      if (this.contractId) {
        this.isEditMode = true;
      }
    });
    
    // Tải dữ liệu dropdown
    this.loadDropdownData();
  }

  // Khởi tạo form trống ngay từ đầu để tránh lỗi null
  initEmptyForms(): void {
    this.contractForm = this.fb.group({
      employeeId: [null, Validators.required],
      departmentId: [null, Validators.required],
      positionId: [null, Validators.required],
      contractCode: ['', Validators.required],
      contractTypeId: [null, Validators.required],
      salary: [0, [Validators.required, Validators.min(0)]],
      notes: [''],
      startDate: [null, Validators.required],
      endDate: [null],
      signedDate: [null],
      workingTime: [''],
      jobDescription: [''],
      benefits: [''],
      filePath: [''],
      terms: [''],
      status: [0, Validators.required],
    });

    this.addendumForm = this.fb.group({
      title: ['', Validators.required],
      addendumContent: ['', Validators.required],
      effectiveDate: [null, Validators.required],
      changeField: [''],
      salary: [0],
      endDate: [null],
      positionId: [null],
      departmentId: [null]
    });
  }

  // Getter cho danh sách phụ lục đã lọc (không được đánh dấu để xóa)
  get filteredAddendums(): any[] {
    return this.contractAddendums.filter(addendum => addendum['actionType'] !== 'D');
  }

  initFormsWithData(): void {
    
    this.setupAutoComplete();
    
    if (this.isEditMode) {
      this.loadContractData();
    }
  }

  setupAutoComplete(): void {
    this.contractForm.patchValue({ contractCode: this.defaultCode });

  const employeeIdControl = this.contractForm.get('employeeId');
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
      defaultCode: this.contractsService.getNew(),
      statusList: this.lookupService.getSysDmtt("CONTRACT"),
      departmentList: this.lookupService.getDepartments(),
      positionList: this.lookupService.getPositions(),
      contractTypeList: this.lookupService.getContractTypes(),
      employeeList: this.lookupService.getEmployees()
    }).subscribe({
      next: (result) => {
        this.statusList = result.statusList;
        this.departmentList = result.departmentList;
        this.positionList = result.positionList;
        this.contractTypeList = result.contractTypeList;
        this.employeeList = result.employeeList;
        this.defaultCode = result.defaultCode.data;
        
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
  if(this.isEditMode) return;
  const employeeIdControl = this.contractForm.get('employeeId');
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

  loadContractData(): void {
    this.contractsService.getById(this.contractId).subscribe({
      next: (response: any) => {

        if (response.success) {
          const contract = response.data.contractRes;
          this.contractForm.patchValue({
            employeeId: contract.employeeId,
            departmentId: contract.departmentId,
            positionId: contract.positionId,
            contractCode: contract.contractCode,
            contractTypeId: contract.contractTypeId,
            salary: contract.salary,
            notes: contract.notes,
            startDate: this.formatDateForInput(contract.startDate),
            endDate: contract.endDate ? this.formatDateForInput(contract.endDate) : null,
            signedDate: contract.signedDate ? this.formatDateForInput(contract.signedDate) : null,
            workingTime: contract.workingTime,
            jobDescription: contract.jobDescription,
            benefits: contract.benefits,
            filePath: contract.filePath,
            terms: contract.terms,
            status : contract.status
          });
          setTimeout(() => {
          this.contractForm.get('employeeId')?.setValue(contract.employeeId);
        });
          // Load addendums and history if they are included in the response
          if (response.data.contractAddendumRes) {
            this.contractAddendums = Array.isArray(response.data.contractAddendumRes) 
              ? response.data.contractAddendumRes 
              : [response.data.contractAddendumRes];
            
            // Format dates for addendums
            this.contractAddendums.forEach(addendum => {
              if (addendum.effectiveDate) {
                addendum.effectiveDate = new Date(addendum.effectiveDate);
              }
              if (addendum.endDate) {
                addendum.endDate = new Date(addendum.endDate);
              }
              // Initialize actionType if not present
              if (!addendum['actionType']) {
                addendum['actionType'] = 'N'; // N for No changes (original data)
              }
            });
          }
          
          if (response.data.contractHistoryRes) {
            this.contractHistory = Array.isArray(response.data.contractHistoryRes) 
              ? response.data.contractHistoryRes 
              : [response.data.contractHistoryRes];
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

  saveContract(): void {
    if (this.contractForm.invalid) {
      // Mark all fields as touched to show validation errors
      Object.keys(this.contractForm.controls).forEach(key => {
        const control = this.contractForm.get(key);
        if (control) {
          control.markAsTouched();
        }
      });
      this.showErrorMessage('Please correct the errors in the form');
      return;
    }

    const contractData: ContractDataset = {
      contractDTO: {
        ...this.contractForm.value
      },
      contractAddendumDTOs: this.contractAddendums.map(addendum => ({
        id: 0,
        contractId: addendum.contractId,
        title: addendum.title,
        addendumContent: addendum.addendumContent,
        effectiveDate: addendum.effectiveDate,
        changeField: addendum.changeField,
        salary: addendum.salary,
        endDate: addendum.endDate,
        positionId: addendum.positionId,
        departmentId: addendum.departmentId,
        actionType: addendum['actionType'] || 'A' // Default to Add if not specified
      }))
    };

    if (this.isEditMode) {
      this.contractsService.update(this.contractId, contractData).subscribe({
        next: (response: any) => {
          if (response.success) {
            this.showSuccessMessage('Contract updated successfully');
            this.router.navigate(['/employee-contracts']);
          } else {
            this.showErrorMessage(response.message || 'Failed to update contract');
          }
        },
        error: (error) => {
          this.showErrorMessage('Failed to update contract');
          console.error(error);
        }
      });
    } else {
      this.contractsService.create(contractData).subscribe({
        next: (response: any) => {
          if (response.success) {
            this.showSuccessMessage('Contract created successfully');
            this.router.navigate(['/employee-contracts']);
          } else {
            this.showErrorMessage(response.message || 'Failed to create contract');
          }
        },
        error: (error) => {
          this.showErrorMessage('Failed to create contract');
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

  
 

  editAddendum(addendum: ContractAddendumDTO): void {
    this.openAddendumModal(addendum);
  }

  deleteAddendum(addendum: ContractAddendumDTO): void {
    if (confirm('Are you sure you want to delete this addendum?')) {
      if (addendum.id > 0) {
        // Existing addendum - mark for deletion
        addendum['actionType'] = 'D'; 
      } else {
        // New addendum - remove from array
        this.contractAddendums = this.contractAddendums.filter(a => a.id !== addendum.id);
      }
    }
  }

  cancel(): void {
    this.router.navigate(['/employee-contracts']);
  }

  changeTab(index: number): void {
    this.activeTab = index;
  }
  onChangeFieldSelected(): void {
  const changeField = this.addendumForm.get('changeField')?.value;
  
  // Reset all fields first
  this.addendumForm.patchValue({
    departmentId: null,
    positionId: null,
    salary: null,
    endDate: null
  });
  
  // Update validators based on selected field
  if (changeField === 'department') {
    this.addendumForm.get('departmentId')?.setValidators([Validators.required]);
    this.addendumForm.get('positionId')?.clearValidators();
    this.addendumForm.get('salary')?.clearValidators();
    this.addendumForm.get('endDate')?.clearValidators();
  } 
  else if (changeField === 'position') {
    this.addendumForm.get('departmentId')?.clearValidators();
    this.addendumForm.get('positionId')?.setValidators([Validators.required]);
    this.addendumForm.get('salary')?.clearValidators();
    this.addendumForm.get('endDate')?.clearValidators();
  }
  else if (changeField === 'salary') {
    this.addendumForm.get('departmentId')?.clearValidators();
    this.addendumForm.get('positionId')?.clearValidators();
    this.addendumForm.get('salary')?.setValidators([Validators.required, Validators.min(0)]);
    this.addendumForm.get('endDate')?.clearValidators();
  }
  else if (changeField === 'endDate') {
    this.addendumForm.get('departmentId')?.clearValidators();
    this.addendumForm.get('positionId')?.clearValidators();
    this.addendumForm.get('salary')?.clearValidators();
    this.addendumForm.get('endDate')?.setValidators([Validators.required]);
  }
  
  // Update form controls with new validators
  this.addendumForm.get('departmentId')?.updateValueAndValidity();
  this.addendumForm.get('positionId')?.updateValueAndValidity();
  this.addendumForm.get('salary')?.updateValueAndValidity();
  this.addendumForm.get('endDate')?.updateValueAndValidity();
}

// Update the openAddendumModal method to handle the changeField
openAddendumModal(addendum?: ContractAddendumDTO): void {
  // Reset the form first
  this.addendumForm.reset({
    title: '',
    addendumContent: '',
    effectiveDate: null,
    changeField: '',
    salary: null,
    endDate: null,
    positionId: null,
    departmentId: null
  });
  
  if (addendum) {
    this.editingAddendumId = addendum.id;
    
    // Set basic values
    this.addendumForm.patchValue({
      title: addendum.title,
      addendumContent: addendum.addendumContent,
      effectiveDate: addendum.effectiveDate ? this.formatDateForInput(addendum.effectiveDate) : null
    });
    
    // Determine which field was changed based on the data
    let changeField = '';
    if (addendum.departmentId) {
      changeField = 'department';
      this.addendumForm.patchValue({ departmentId: addendum.departmentId });
    } else if (addendum.positionId) {
      changeField = 'position';
      this.addendumForm.patchValue({ positionId: addendum.positionId });
    } else if (addendum.salary) {
      changeField = 'salary';
      this.addendumForm.patchValue({ salary: addendum.salary });
    } else if (addendum.endDate) {
      changeField = 'endDate';
      this.addendumForm.patchValue({ 
        endDate: addendum.endDate ? this.formatDateForInput(addendum.endDate) : null 
      });
    }
    
    // Set the change field and trigger the selection logic
    this.addendumForm.patchValue({ changeField: changeField });
    this.onChangeFieldSelected();
    
  } else {
    this.editingAddendumId = null;
  }
  
  this.displayAddendumModal = true;
}

// Also update the saveAddendum method to properly handle the data
saveAddendum(): void {
  if (this.addendumForm.invalid) {
    Object.keys(this.addendumForm.controls).forEach(key => {
      const control = this.addendumForm.get(key);
      if (control) {
        control.markAsTouched();
      }
    });
    return;
  }

  // Get change field value
  const changeField = this.addendumForm.get('changeField')?.value;
  
  // Build addendum data with correct field translated for display
  let changeFieldDisplay = '';
  switch (changeField) {
    case 'department':
      changeFieldDisplay = 'Phòng ban';
      break;
    case 'position':
      changeFieldDisplay = 'Chức vụ';
      break;
    case 'salary':
      changeFieldDisplay = 'Lương';
      break;
    case 'endDate':
      changeFieldDisplay = 'Ngày hết hạn';
      break;
  }
  
  const addendumData = {
    ...this.addendumForm.value,
    changeField: changeFieldDisplay
  };

  if (this.editingAddendumId) {
    // Update existing addendum
    const index = this.contractAddendums.findIndex(a => a.id === this.editingAddendumId);
    if (index !== -1) {
      // Set action type to 'E' for Edit/Update if it's an existing record (positive ID)
      const actionType = this.editingAddendumId > 0 ? 'E' : 'A';
      
      this.contractAddendums[index] = {
        ...this.contractAddendums[index],
        ...addendumData,
        id: this.editingAddendumId,
        contractId: this.contractId,
        actionType: actionType
      };
    }
  } else {
    // Add new addendum with a temporary ID (negative to distinguish from server IDs)
    const tempId = -1 * (this.contractAddendums.length + 1);
    this.contractAddendums.push({
      ...addendumData,
      id: tempId,
      contractId: this.contractId || 0,
      actionType: 'A'
    } as ContractAddendumDTO);
  }

  this.displayAddendumModal = false;
  this.editingAddendumId = null;
}
  
}