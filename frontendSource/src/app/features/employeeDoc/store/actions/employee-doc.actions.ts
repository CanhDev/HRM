import { createAction, props } from '@ngrx/store';
import { EducationDTO, EmergencyContactDTO, Employee, EmployeeCreateRequest, EmployeeDataResponse, EmployeeResponse, WorkExperienceDTO } from '../../models/entity';

import { ApiResponseBasic, PagedResult, SearchRequest, DateRange, ApiResponse } from 'src/app/core/models/base';

export const loadEmployees = createAction(
  '[Employee] Load Employees',
  props<{ searchRequest: SearchRequest<Employee> }>()
);

export const loadEmployeesSuccess = createAction(
  '[Employee] Load Employees Success',
  props<{ employees: Employee[]; listResponse: PagedResult<Employee> | null }>()
);

export const loadEmployeesFailure = createAction(
  '[Employee] Load Employees Failure',
  props<{ error: any }>()
);

export const loadEmployee = createAction(
  '[Employee] Load Employee',
  props<{ id: number }>()
);

export const loadEmployeeSuccess = createAction(
  '[Employee] Load Employee Success',
  props<{ employee: ApiResponse<EmployeeDataResponse> }>()
);

export const loadEmployeeFailure = createAction(
  '[Employee] Load Employee Failure',
  props<{ error: any }>()
);

// Create Employee Actions
export const createEmployee = createAction(
  '[Employee] Create Employee',
  props<{ employee: EmployeeCreateRequest }>()
);

export const createEmployeeSuccess = createAction(
  '[Employee] Create Employee Success',
  props<{ response: ApiResponse<EmployeeResponse> }>()
);

export const createEmployeeFailure = createAction(
  '[Employee] Create Employee Failure',
  props<{ error: any }>()
);

// Update Employee Actions
export const updateEmployee = createAction(
  '[Employee] Update Employee',
  props<{ id: number, employee: EmployeeCreateRequest }>()
);

export const updateEmployeeSuccess = createAction(
  '[Employee] Update Employee Success',
  props<{ response: ApiResponse<EmployeeResponse> }>()
);

export const updateEmployeeFailure = createAction(
  '[Employee] Update Employee Failure',
  props<{ error: any }>()
);
export const deleteEmployee = createAction(
  '[Employee] Delete Employee',
  props<{ id: number }>()
);

export const deleteEmployeeSuccess = createAction(
  '[Employee] Delete Employee Success',
  props<{ id: number }>()
);

export const deleteEmployeeFailure = createAction(
  '[Employee] Delete Employee Failure',
  props<{ error: any }>()
);

export const setSearchRequest = createAction(
  '[Employee] Set Search Request',
  props<{ searchRequest: SearchRequest<Employee> }>()
);
// Form-specific actions for emergency contacts
export const addEmergencyContact = createAction(
  '[Employee Form] Add Emergency Contact',
  props<{ contact: EmergencyContactDTO }>()
);

export const updateEmergencyContact = createAction(
  '[Employee Form] Update Emergency Contact',
  props<{ index: number, contact: EmergencyContactDTO }>()
);

export const deleteEmergencyContact = createAction(
  '[Employee Form] Delete Emergency Contact',
  props<{ index: number }>()
);

// Form-specific actions for education
export const addEducation = createAction(
  '[Employee Form] Add Education',
  props<{ education: EducationDTO }>()
);

export const updateEducation = createAction(
  '[Employee Form] Update Education',
  props<{ index: number, education: EducationDTO }>()
);

export const deleteEducation = createAction(
  '[Employee Form] Delete Education',
  props<{ index: number }>()
);

// Form-specific actions for work experience
export const addWorkExperience = createAction(
  '[Employee Form] Add Work Experience',
  props<{ experience: WorkExperienceDTO }>()
);

export const updateWorkExperience = createAction(
  '[Employee Form] Update Work Experience',
  props<{ index: number, experience: WorkExperienceDTO }>()
);

export const deleteWorkExperience = createAction(
  '[Employee Form] Delete Work Experience',
  props<{ index: number }>()
);

// Reset form state
export const resetEmployeeForm = createAction(
  '[Employee Form] Reset Form'
);