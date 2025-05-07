// src/app/features/employee/containers/employee-update/employee-update.component.ts

import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable, Subject } from 'rxjs';
import { shareReplay, takeUntil, filter, take } from 'rxjs/operators';
import { ActivatedRoute, Router } from '@angular/router';
import { 
  EmployeeDTO, 
  EmergencyContactDTO, 
  EducationDTO, 
  WorkExperienceDTO,
  EmployeeCreateRequest,
  EmployeeDataset,
  EmployeeDataResponse
} from '../../../models/entity';
import * as EmployeeActions from '../../../store/actions/employee-doc.actions';
import * as fromEmployee from '../../../store/selectors/employee-doc.selectors';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';
import { Modal } from 'bootstrap';

@Component({
  selector: 'app-employee-update',
  templateUrl: './employee-update.component.html',
  styleUrls: ['./employee-update.component.css']
})
export class EmployeeUpdateComponent implements OnInit, OnDestroy {
  // Similar form declarations as in create component
  employeeForm: FormGroup;
  emergencyContactForm: FormGroup;
  educationForm: FormGroup;
  workExperienceForm: FormGroup;
  
  // Observables for state management
  loading$: Observable<boolean>;
  error$: Observable<any>;
  updateSuccess$: Observable<boolean>;
  employee$: Observable<EmployeeDataResponse | null>;
  
  // File handling properties
  selectedImageFile: File | null = null;
  imagePreview: string | ArrayBuffer | null = null;
  
  // Modal state tracking properties
  isEditingEmergencyContact: boolean = false;
  isEditingEducation: boolean = false;
  isEditingWorkExperience: boolean = false;
  
  currentEmergencyContactIndex: number = -1;
  currentEducationIndex: number = -1;
  currentWorkExperienceIndex: number = -1;
  
  // Bootstrap modals
  private emergencyContactModal: Modal | null = null;
  private educationModal: Modal | null = null;
  private workExperienceModal: Modal | null = null;
  
  employeeId: number = 0;
  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private lookupService: LookupService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    // Select state from store
    this.loading$ = this.store.select(fromEmployee.selectLoading);
    this.error$ = this.store.select(fromEmployee.selectError);
    this.updateSuccess$ = this.store.select(fromEmployee.selectSaveSuccess);
    this.employee$ = this.store.select(fromEmployee.selectCurrentEmployee) ;
    
    // Initialize forms - same as create component
    this.employeeForm = this.createEmployeeForm();
    this.emergencyContactForm = this.createEmergencyContactForm();
    this.educationForm = this.createEducationForm();
    this.workExperienceForm = this.createWorkExperienceForm();
  }
  
  // Lookup service observables - same as create component
  departments$ = this.lookupService.getDepartments().pipe(shareReplay(1));
  positions$ = this.lookupService.getPositions().pipe(shareReplay(1));
  statusList$ = this.lookupService.getSysListOptions("Employee", "status").pipe(shareReplay(1));
  contractTypes$ = this.lookupService.getContractTypes().pipe(shareReplay(1));

  ngOnInit(): void {
    // Get employee ID from route params
    this.route.params.pipe(
      take(1),
      takeUntil(this.destroy$)
    ).subscribe(params => {
      if (params['id']) {
        this.employeeId = +params['id'];
        // Dispatch action to load employee data
        this.store.dispatch(EmployeeActions.loadEmployee({ id: this.employeeId }));
      } else {
        // Handle case where ID is not provided
        this.router.navigate(['/employees']);
      }
    });
    
    // Subscribe to employee data and populate form
    this.employee$.pipe(
      filter(employee => !!employee),
      takeUntil(this.destroy$)
    ).subscribe(employee => {
      if (employee) {
        this.populateForm(employee);
      }
    });
    
    // Subscribe to update success state
    this.updateSuccess$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(success => {
      if (success) {
        // Form was successfully updated
        // Additional logic could be added here
      }
    });
    
    // Initialize Bootstrap modals
    setTimeout(() => {
      this.initModals();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    // Reset state when leaving
    this.store.dispatch(EmployeeActions.resetEmployeeForm());
  }
  
  // Initialize Bootstrap modals - same as create component
  initModals(): void {
    const emergencyContactModalEl = document.getElementById('emergencyContactModal');
    const educationModalEl = document.getElementById('educationModal');
    const workExperienceModalEl = document.getElementById('workExperienceModal');
    
    if (emergencyContactModalEl) {
      this.emergencyContactModal = new Modal(emergencyContactModalEl);
    }
    
    if (educationModalEl) {
      this.educationModal = new Modal(educationModalEl);
    }
    
    if (workExperienceModalEl) {
      this.workExperienceModal = new Modal(workExperienceModalEl);
    }
  }

  // Form creation methods - same as create component
  createEmployeeForm(): FormGroup {
    return this.fb.group({
      employeeDTO: this.fb.group({
        employeeCode: ['', Validators.required],
        fullName: ['', [Validators.required, Validators.maxLength(100)]],
        dob: [null],
        gender: ['Nam', Validators.maxLength(10)],
        idCardNumber: ['', Validators.maxLength(20)],
        idCardIssueDate: [null],
        idCardIssuePlace: [''],
        taxCode: [''],
        bankAccountNumber: [''],
        bankName: [''],
        bankBranch: [''],
        joinDate: [null],
        phone: ['', Validators.maxLength(20)],
        email: ['', [Validators.email, Validators.maxLength(100)]],
        address: ['', Validators.maxLength(255)],
        netSalary: [0],
        departmentId: ['2'],
        positionId: ['3'],
        contractTypeId: ['2'],
        imageUrl: [''],
        status: [1]
      }),
      emergencyContacts: this.fb.array([]),
      educationList: this.fb.array([]),
      workExperienceList: this.fb.array([])
    });
  }

  get emergencyContacts(): FormArray {
    return this.employeeForm.get('emergencyContacts') as FormArray;
  }

  get educationList(): FormArray {
    return this.employeeForm.get('educationList') as FormArray;
  }

  get workExperienceList(): FormArray {
    return this.employeeForm.get('workExperienceList') as FormArray;
  }

  // Form group creation methods - same as create component
  createEmergencyContactForm(): FormGroup {
    return this.fb.group({
      id: [0],
      employeeId: [null],
      relationship: [''],
      fullName: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      address: [''],
      actionType: ['E'],  // A for Add
      status: [1]
    });
  }

  createEducationForm(): FormGroup {
    return this.fb.group({
      id: [0],
      employeeId: [null],
      degree: ['', Validators.required],
      major: ['', Validators.required],
      school: ['', Validators.required],
      startDate: [null],
      endDate: [null],
      gpa: [null],
      description: [''],
      actionType: ['E'],  // A for Add
      status: [1]
    });
  }

  createWorkExperienceForm(): FormGroup {
    return this.fb.group({
      id: [0],
      employeeId: [null],
      companyName: ['', Validators.required],
      position: ['', Validators.required],
      startDate: [null],
      endDate: [null],
      description: [''],
      actionType: ['E'],  // A for Add
      status: [1]
    });
  }

  // Methods to populate form with employee data
  populateForm(employee: EmployeeDataResponse): void {
    // Populate main employee form group
    const employeeForm = this.employeeForm.get('employeeDTO');
    if (employeeForm) {
      employeeForm.patchValue({
        ...employee.employeeRes,
        // Ensure numeric values are correctly converted,
        dob: this.lookupService.formatDate(employee.employeeRes.dob.toString()),
        idCardIssueDate: this.lookupService.formatDate(employee.employeeRes.idCardIssueDate.toString()),
        joinDate: this.lookupService.formatDate(employee.employeeRes.joinDate.toString()),
        departmentId: employee.employeeRes.departmentId,
        positionId: employee.employeeRes.positionId,
        contractTypeId: employee.employeeRes.contractTypeId,
        status: employee.employeeRes.status
      });
    }
    
    // Set image preview if available
    if (employee.employeeRes.imageUrl) {
      this.imagePreview = employee.employeeRes.imageUrl;
    }
    
    // Populate emergency contacts
    if (employee.emergencyContactList && employee.emergencyContactList.length > 0) {
      this.emergencyContacts.clear();
      employee.emergencyContactList.forEach(contact => {
        const contactForm = this.fb.group({
          id: [contact.id || 0],
          employeeId: [contact.employeeId],
          relationship: [contact.relationship || ''],
          fullName: [contact.fullName || '', Validators.required],
          phoneNumber: [contact.phoneNumber || '', Validators.required],
          address: [contact.address || ''],
          actionType: ['E'],  // N for No change initially
          status: [contact.status || 1]
        });
        this.emergencyContacts.push(contactForm);
      });
    }
    
    // Populate education list
    if (employee.educationList && employee.educationList.length > 0) {
      this.educationList.clear();
      employee.educationList.forEach(education => {
        const educationForm = this.fb.group({
          id: [education.id || 0],
          employeeId: [education.employeeId],
          degree: [education.degree || '', Validators.required],
          major: [education.major || '', Validators.required],
          school: [education.school || '', Validators.required],
          startDate: [education.startDate],
          endDate: [education.endDate],
          gpa: [education.gpa],
          description: [education.description || ''],
          actionType: ['E'],  // N for No change initially
          status: [education.status || 1]
        });
        this.educationList.push(educationForm);
      });
    }
    
    // Populate work experience list
    if (employee.workExperienceList && employee.workExperienceList.length > 0) {
      this.workExperienceList.clear();
      employee.workExperienceList.forEach(experience => {
        const experienceForm = this.fb.group({
          id: [experience.id || 0],
          employeeId: [experience.employeeId],
          companyName: [experience.companyName || '', Validators.required],
          position: [experience.position || '', Validators.required],
          startDate: [experience.startDate],
          endDate: [experience.endDate],
          description: [experience.description || ''],
          actionType: ['E'],  // N for No change initially
          status: [experience.status || 1]
        });
        this.workExperienceList.push(experienceForm);
      });
    }
  }

  // Add/remove methods for form arrays - same as create component
  addEmergencyContact(): void {
    this.emergencyContacts.push(this.createEmergencyContactForm());
  }

  addEducation(): void {
    this.educationList.push(this.createEducationForm());
  }

  addWorkExperience(): void {
    this.workExperienceList.push(this.createWorkExperienceForm());
  }

  removeEmergencyContact(index: number): void {
    const contact = this.emergencyContacts.at(index);
    if (contact.get('id')?.value > 0) {
      // Mark for deletion rather than remove if it exists in DB
      contact.patchValue({ actionType: 'D', status: -1 });
    } else {
      // Remove completely if it's new and not in DB
      this.emergencyContacts.removeAt(index);
    }
  }

  removeEducation(index: number): void {
    const education = this.educationList.at(index);
    if (education.get('id')?.value > 0) {
      // Mark for deletion rather than remove if it exists in DB
      education.patchValue({ actionType: 'D', status: -1 });
    } else {
      // Remove completely if it's new and not in DB
      this.educationList.removeAt(index);
    }
  }

  removeWorkExperience(index: number): void {
    const experience = this.workExperienceList.at(index);
    if (experience.get('id')?.value > 0) {
      // Mark for deletion rather than remove if it exists in DB
      experience.patchValue({ actionType: 'D', status: -1 });
    } else {
      // Remove completely if it's new and not in DB
      this.workExperienceList.removeAt(index);
    }
  }

  // File handling methods - same as create component
  onFileSelected(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.selectedImageFile = file;
      
      // Preview the selected image
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result;
      };
      reader.readAsDataURL(file);
    }
  }
  
  // Modal methods - mostly same as create component
  openEmergencyContactModal(): void {
    // Reset form
    this.isEditingEmergencyContact = false;
    this.currentEmergencyContactIndex = -1;
    this.emergencyContactForm.reset({
      id: 0,
      employeeId: this.employeeId,
      relationship: '',
      fullName: '',
      phoneNumber: '',
      address: '',
      actionType: 'A',
      status: 1
    });
    
    if (this.emergencyContactModal) {
      this.emergencyContactModal.show();
    }
  }
  
  editEmergencyContact(index: number): void {
    // Similar to create but ensure we're setting actionType to 'U' for update
    this.isEditingEmergencyContact = true;
    this.currentEmergencyContactIndex = index;
    
    const contact = this.emergencyContacts.at(index);
    this.emergencyContactForm.patchValue({
      id: contact.get('id')?.value || 0,
      employeeId: contact.get('employeeId')?.value || this.employeeId,
      relationship: contact.get('relationship')?.value,
      fullName: contact.get('fullName')?.value,
      phoneNumber: contact.get('phoneNumber')?.value,
      address: contact.get('address')?.value,
      actionType: 'E',  // U for Update
      status: contact.get('status')?.value || 1
    });
    
    if (this.emergencyContactModal) {
      this.emergencyContactModal.show();
    }
  }
  
  saveEmergencyContact(): void {
    // Same validation logic as create
    if (this.emergencyContactForm.invalid) {
      this.markFormGroupTouched(this.emergencyContactForm);
      return;
    }
    
    if (this.isEditingEmergencyContact && this.currentEmergencyContactIndex >= 0) {
      // Update existing contact
      this.emergencyContacts.at(this.currentEmergencyContactIndex).patchValue(this.emergencyContactForm.value);
    } else {
      // Add new contact
      this.addEmergencyContact();
      this.emergencyContacts.at(this.emergencyContacts.length - 1).patchValue(this.emergencyContactForm.value);
    }
    
    if (this.emergencyContactModal) {
      this.emergencyContactModal.hide();
    }
  }
  get isAllEmergencyContactsDeleted(): boolean {
    return (
      this.emergencyContacts.length === 0 ||
      this.emergencyContacts.controls.every(
        (c) => c.get('actionType')?.value === 'D'
      )
    );
  }
  get isAlleducationListDeleted(): boolean {
    return (
      this.educationList.length === 0 ||
      this.educationList.controls.every(
        (c) => c.get('actionType')?.value === 'D'
      )
    );
  }
  get isAllworkExperienceListDeleted(): boolean {
    return (
      this.workExperienceList.length === 0 ||
      this.workExperienceList.controls.every(
        (c) => c.get('actionType')?.value === 'D'
      )
    );
  }
  // Education Modal Methods - similar to create component
  openEducationModal(): void {
    this.isEditingEducation = false;
    this.currentEducationIndex = -1;
    this.educationForm.reset({
      id: 0,
      employeeId: this.employeeId,
      degree: '',
      major: '',
      school: '',
      startDate: null,
      endDate: null,
      gpa: null,
      description: '',
      actionType: 'A',
      status: 1
    });
    
    if (this.educationModal) {
      this.educationModal.show();
    }
  }
  
  editEducation(index: number): void {
    this.isEditingEducation = true;
    this.currentEducationIndex = index;
    
    const education = this.educationList.at(index);
    this.educationForm.patchValue({
      id: education.get('id')?.value || 0,
      employeeId: education.get('employeeId')?.value || this.employeeId,
      degree: education.get('degree')?.value,
      major: education.get('major')?.value,
      school: education.get('school')?.value,
      startDate: this.lookupService.formatDate(education.get('startDate')?.value),
      endDate: this.lookupService.formatDate(education.get('endDate')?.value),
      gpa: education.get('gpa')?.value,
      description: education.get('description')?.value,
      actionType: 'E',  
      status: education.get('status')?.value || 1
    });
    
    if (this.educationModal) {
      this.educationModal.show();
    }
  }
  
  saveEducation(): void {
    // Same as create component
    if (this.educationForm.invalid) {
      this.markFormGroupTouched(this.educationForm);
      return;
    }
    
    if (this.isEditingEducation && this.currentEducationIndex >= 0) {
      // Update existing education
      this.educationList.at(this.currentEducationIndex).patchValue(this.educationForm.value);
    } else {
      // Add new education
      this.addEducation();
      this.educationList.at(this.educationList.length - 1).patchValue(this.educationForm.value);
    }
    
    if (this.educationModal) {
      this.educationModal.hide();
    }
  }
  
  // Work Experience Modal Methods - similar to create component
  openWorkExperienceModal(): void {
    this.isEditingWorkExperience = false;
    this.currentWorkExperienceIndex = -1;
    this.workExperienceForm = this.createWorkExperienceForm();
    this.workExperienceForm.patchValue({
      employeeId: this.employeeId
    });
    
    if (this.workExperienceModal) {
      this.workExperienceModal.show();
    }
  }
  
  editWorkExperience(index: number): void {
    // Similar to create with actionType = 'U'
    this.isEditingWorkExperience = true;
    this.currentWorkExperienceIndex = index;
    
    const experience = this.workExperienceList.at(index);
    this.workExperienceForm.patchValue({
      id: experience.get('id')?.value || 0,
      employeeId: experience.get('employeeId')?.value || this.employeeId,
      companyName: experience.get('companyName')?.value,
      position: experience.get('position')?.value,
      startDate: this.lookupService.formatDate(experience.get('startDate')?.value),
      endDate: this.lookupService.formatDate(experience.get('endDate')?.value),
      description: experience.get('description')?.value,
      actionType: 'E',  // U for Update
      status: experience.get('status')?.value || 1
    });
    
    if (this.workExperienceModal) {
      this.workExperienceModal.show();
    }
  }
  
  saveWorkExperience(): void {
    // Same as create component
    if (this.workExperienceForm.invalid) {
      this.markFormGroupTouched(this.workExperienceForm);
      return;
    }
    
    if (this.isEditingWorkExperience && this.currentWorkExperienceIndex >= 0) {
      // Update existing work experience
      this.workExperienceList.at(this.currentWorkExperienceIndex).patchValue(this.workExperienceForm.value);
    } else {
      // Add new work experience
      this.addWorkExperience();
      this.workExperienceList.at(this.workExperienceList.length - 1).patchValue(this.workExperienceForm.value);
    }
    
    if (this.workExperienceModal) {
      this.workExperienceModal.hide();
    }
  }

  // Submit handler for update
  onSubmit(): void {
    if (this.employeeForm.invalid) {
      // Mark all fields as touched to trigger validation messages
      this.markFormGroupTouched(this.employeeForm);
      return;
    }

    const employeeData = this.prepareEmployeeData();
    this.store.dispatch(EmployeeActions.updateEmployee({id:  this.employeeId ,employee: employeeData }));
  }

  // Helper to mark all form controls as touched - same as create component
  markFormGroupTouched(formGroup: FormGroup): void {
    // Same as create component - you can copy this
  }

  // Prepares the data for API submission
  prepareEmployeeData(): EmployeeCreateRequest {
    const formValue = this.employeeForm.value;
    const employeeDTO: EmployeeDTO = {
      ...formValue.employeeDTO,
      id: this.employeeId  // Make sure ID is set for update
    };
    
    // Convert form arrays to DTOs
    const emergencyContacts: EmergencyContactDTO[] = this.emergencyContacts.value;
    const educationList: EducationDTO[] = this.educationList.value;
    const workExperienceList: WorkExperienceDTO[] = this.workExperienceList.value;
    
    // Create request object
    const request: EmployeeCreateRequest = {
      employeeDTO: employeeDTO,
      emergencyContactsJson: JSON.stringify(emergencyContacts),
      educationListJson: JSON.stringify(educationList),
      workExperienceListJson: JSON.stringify(workExperienceList),
      imageFile: this.selectedImageFile || undefined
    };
    
    return request;
  }
}