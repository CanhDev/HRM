// src/app/features/employee/containers/employee-create/employee-create.component.ts

import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { Observable, Subject } from 'rxjs';
import { shareReplay, takeUntil } from 'rxjs/operators';
import { 
  EmployeeDTO, 
  EmergencyContactDTO, 
  EducationDTO, 
  WorkExperienceDTO,
  EmployeeCreateRequest
} from '../../../models/entity';
import * as EmployeeActions from '../../../store/actions/employee-doc.actions';
import * as fromEmployee from '../../../store/selectors/employee-doc.selectors';
import { LookupService } from 'src/app/core/services/lookup/lookup.service';
import { Modal } from 'bootstrap';

@Component({
  selector: 'app-employee-create',
  templateUrl: './employee-create.component.html',
  styleUrls: ['./employee-create.component.css']
})
export class EmployeeCreateComponent implements OnInit, OnDestroy {
  employeeForm: FormGroup;
  emergencyContactForm: FormGroup;
  educationForm: FormGroup;
  workExperienceForm: FormGroup;
  
  loading$: Observable<boolean>;
  error$: Observable<any>;
  saveSuccess$: Observable<boolean>;
  
  selectedImageFile: File | null = null;
  imagePreview: string | ArrayBuffer | null = null;
  
  isEditingEmergencyContact: boolean = false;
  isEditingEducation: boolean = false;
  isEditingWorkExperience: boolean = false;
  
  currentEmergencyContactIndex: number = -1;
  currentEducationIndex: number = -1;
  currentWorkExperienceIndex: number = -1;
  
  private emergencyContactModal: Modal | null = null;
  private educationModal: Modal | null = null;
  private workExperienceModal: Modal | null = null;
  
  private destroy$ = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private lookupService: LookupService
  ) {
    this.loading$ = this.store.select(fromEmployee.selectLoading);
    this.error$ = this.store.select(fromEmployee.selectError);
    this.saveSuccess$ = this.store.select(fromEmployee.selectSaveSuccess);
    // Initialize the form
    this.employeeForm = this.createEmployeeForm();
    this.emergencyContactForm = this.createEmergencyContactForm();
    this.educationForm = this.createEducationForm();
    this.workExperienceForm = this.createWorkExperienceForm();
  }
    departments$ = this.lookupService.getDepartments().pipe(shareReplay(1));
    positions$ = this.lookupService.getPositions().pipe(shareReplay(1));
    statusList$ = this.lookupService.getSysListOptions("Employee", "status").pipe(shareReplay(1));
    contractTypes$ = this.lookupService.getContractTypes().pipe(shareReplay(1));

  ngOnInit(): void {
    // Reset the form state when component initializes
    this.store.dispatch(EmployeeActions.resetEmployeeForm());
    
    // Subscribe to success state to handle post-save actions
    this.saveSuccess$.pipe(
      takeUntil(this.destroy$)
    ).subscribe(success => {
      if (success) {
        // Form was successfully submitted
        // Additional logic could be added here
      }
    });
    
    // Initialize Bootstrap modals after view is ready
    setTimeout(() => {
      this.initModals();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
  
  // Initialize Bootstrap modals
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

  // Creates the main employee form
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

  // Form arrays getters
  get emergencyContacts(): FormArray {
    return this.employeeForm.get('emergencyContacts') as FormArray;
  }

  get educationList(): FormArray {
    return this.employeeForm.get('educationList') as FormArray;
  }

  get workExperienceList(): FormArray {
    return this.employeeForm.get('workExperienceList') as FormArray;
  }

  // Creates a new emergency contact form group
  createEmergencyContactForm(): FormGroup {
    return this.fb.group({
      id: [0],
      employeeId: [null],
      relationship: [''],
      fullName: ['', Validators.required],
      phoneNumber: ['', Validators.required],
      address: [''],
      actionType: ['A'],  // A for Add
      status: [1]
    });
  }

  // Creates a new education form group
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
      actionType: ['A'],  // A for Add
      status: [1]
    });
  }

  // Creates a new work experience form group
  createWorkExperienceForm(): FormGroup {
    return this.fb.group({
      id: [0],
      employeeId: [null],
      companyName: ['', Validators.required],
      position: ['', Validators.required],
      startDate: [null],
      endDate: [null],
      description: [''],
      actionType: ['A'],  // A for Add
      status: [1]
    });
  }

  // Adds a new emergency contact
  addEmergencyContact(): void {
    this.emergencyContacts.push(this.createEmergencyContactForm());
  }

  // Adds a new education entry
  addEducation(): void {
    this.educationList.push(this.createEducationForm());
  }

  // Adds a new work experience entry
  addWorkExperience(): void {
    this.workExperienceList.push(this.createWorkExperienceForm());
  }

  // Removes an emergency contact
  removeEmergencyContact(index: number): void {
    this.emergencyContacts.removeAt(index);
  }

  // Removes an education entry
  removeEducation(index: number): void {
    this.educationList.removeAt(index);
  }

  // Removes a work experience entry
  removeWorkExperience(index: number): void {
    this.workExperienceList.removeAt(index);
  }

  // Handles file input change
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
  
  // Emergency Contact Modal Methods
  openEmergencyContactModal(): void {
    this.isEditingEmergencyContact = false;
    this.currentEmergencyContactIndex = -1;
    this.emergencyContactForm.reset({
      id: 0,
      employeeId: null,
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
    this.isEditingEmergencyContact = true;
    this.currentEmergencyContactIndex = index;
    
    const contact = this.emergencyContacts.at(index);
    this.emergencyContactForm.patchValue({
      id: contact.get('id')?.value || 0,
      employeeId: contact.get('employeeId')?.value,
      relationship: contact.get('relationship')?.value,
      fullName: contact.get('fullName')?.value,
      phoneNumber: contact.get('phoneNumber')?.value,
      address: contact.get('address')?.value,
      actionType: 'U',  // U for Update
      status: contact.get('status')?.value || 1
    });
    
    if (this.emergencyContactModal) {
      this.emergencyContactModal.show();
    }
  }
  
  saveEmergencyContact(): void {
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
  
  // Education Modal Methods
  openEducationModal(): void {
    this.isEditingEducation = false;
    this.currentEducationIndex = -1;
    this.educationForm.reset({
      id: 0,
      employeeId: null,
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
      employeeId: education.get('employeeId')?.value,
      degree: education.get('degree')?.value,
      major: education.get('major')?.value,
      school: education.get('school')?.value,
      startDate: education.get('startDate')?.value,
      endDate: education.get('endDate')?.value,
      gpa: education.get('gpa')?.value,
      description: education.get('description')?.value,
      actionType: 'U',  // U for Update
      status: education.get('status')?.value || 1
    });
    
    if (this.educationModal) {
      this.educationModal.show();
    }
  }
  
  saveEducation(): void {
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
  
  // Work Experience Modal Methods
  openWorkExperienceModal(): void {
    this.isEditingWorkExperience = false;
    this.currentWorkExperienceIndex = -1;
    this.workExperienceForm = this.createWorkExperienceForm();
    
    if (this.workExperienceModal) {
      this.workExperienceModal.show();
    }
  }
  
  editWorkExperience(index: number): void {
    this.isEditingWorkExperience = true;
    this.currentWorkExperienceIndex = index;
    
    const experience = this.workExperienceList.at(index);
    this.workExperienceForm.patchValue({
      id: experience.get('id')?.value || 0,
      employeeId: experience.get('employeeId')?.value,
      companyName: experience.get('companyName')?.value,
      position: experience.get('position')?.value,
      startDate: experience.get('startDate')?.value,
      endDate: experience.get('endDate')?.value,
      description: experience.get('description')?.value,
      actionType: 'U',  // U for Update
      status: experience.get('status')?.value || 1
    });
    
    if (this.workExperienceModal) {
      this.workExperienceModal.show();
    }
  }
  
  saveWorkExperience(): void {
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

  // Submits the form
  onSubmit(): void {
    if (this.employeeForm.invalid) {
      // Mark all fields as touched to trigger validation messages
      this.markFormGroupTouched(this.employeeForm);
      return;
    }

    const employeeData = this.prepareEmployeeData();
    this.store.dispatch(EmployeeActions.createEmployee({ employee: employeeData }));
  }

  // Helper to mark all form controls as touched
  markFormGroupTouched(formGroup: FormGroup): void {
    Object.values(formGroup.controls).forEach(control => {
      control.markAsTouched();

      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  // Prepares the data for API submission
  prepareEmployeeData(): EmployeeCreateRequest {
    const formValue = this.employeeForm.value;
    const employeeDTO: EmployeeDTO = formValue.employeeDTO;
    
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