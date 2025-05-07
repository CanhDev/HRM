import { createReducer, on } from '@ngrx/store';
import * as EmployeeDocActions from '../actions/employee-doc.actions';
import { EducationDTO, EmergencyContactDTO, Employee, EmployeeDataResponse, EmployeeResponse, WorkExperienceDTO } from '../../models/entity';
import { ApiResponseBasic, PagedResult, SearchRequest, DateRange } from 'src/app/core/models/base';

export interface EmployeeState {
  employees: Employee[];
  currentEmployee: EmployeeDataResponse | null;
  emergencyContacts: EmergencyContactDTO[];
  educationList: EducationDTO[];
  workExperienceList: WorkExperienceDTO[];
  saveSuccess: boolean;
  loading: boolean;
  error: any;
  searchRequest: SearchRequest<Employee>;
  listResponse: PagedResult<Employee> | null;
}

export const initialState: EmployeeState = {
  employees: [],
  currentEmployee: null,
  emergencyContacts: [],
  educationList: [],
  workExperienceList: [],
  loading: false,
  error: null,
  searchRequest: {
    globalSearch: '',
    columnFilters: {},
    dateFilters: {},
    sortBy: 'employeeCode',
    sortOrder: 'asc',
    page: 1,
    pageSize: 10
  },
  listResponse: null,
  saveSuccess: false
};

export const employeeReducer = createReducer(
  initialState,
  
  // Set Search Request
  on(EmployeeDocActions.setSearchRequest, (state, { searchRequest }) => ({
    ...state,
    loading: true,
    error: null,
    saveSuccess: false,
    searchRequest: { ...searchRequest }
  })),
  
  // Load Employees
  on(EmployeeDocActions.loadEmployees, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(EmployeeDocActions.loadEmployeesSuccess, (state, { employees, listResponse }) => ({
    ...state,
    employees,
    listResponse,
    loading: false
  })),
  on(EmployeeDocActions.loadEmployeesFailure, (state, { error }) => ({
    ...state,
    error,
    loading: false
  })),
  
  // Load employee
  on(EmployeeDocActions.loadEmployee, (state) => ({
    ...state,
    loading: true,
    error: null,
    saveSuccess: false
  })),
  
  on(EmployeeDocActions.loadEmployeeSuccess, (state, { employee }) => ({
    ...state,
    currentEmployee: employee.data!,
    // Here we would also map related data if included in the response
    loading: false,
    error: null
  })),
  
  on(EmployeeDocActions.loadEmployeeFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error
  })),
  // Create employee
  on(EmployeeDocActions.createEmployee, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  
  on(EmployeeDocActions.createEmployeeSuccess, (state, { response }) => ({
    ...state,
    loading: false,
    error: null,
    saveSuccess: true
  })),
  
  on(EmployeeDocActions.createEmployeeFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
    saveSuccess: false
  })),
  
  // Update employee
  on(EmployeeDocActions.updateEmployee, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  
  on(EmployeeDocActions.updateEmployeeSuccess, (state, { response }) => ({
    ...state,
    loading: false,
    error: null,
    saveSuccess: true
  })),
  
  on(EmployeeDocActions.updateEmployeeFailure, (state, { error }) => ({
    ...state,
    loading: false,
    error,
    saveSuccess: false
  })),
  
  // Emergency Contact actions
  on(EmployeeDocActions.addEmergencyContact, (state, { contact }) => ({
    ...state,
    emergencyContacts: [...state.emergencyContacts, contact]
  })),
  
  on(EmployeeDocActions.updateEmergencyContact, (state, { index, contact }) => ({
    ...state,
    emergencyContacts: state.emergencyContacts.map((item, i) => 
      i === index ? { ...item, ...contact } : item
    )
  })),
  
  on(EmployeeDocActions.deleteEmergencyContact, (state, { index }) => ({
    ...state,
    emergencyContacts: state.emergencyContacts.filter((_, i) => i !== index)
  })),
  
  // Education actions
  on(EmployeeDocActions.addEducation, (state, { education }) => ({
    ...state,
    educationList: [...state.educationList, education]
  })),
  
  on(EmployeeDocActions.updateEducation, (state, { index, education }) => ({
    ...state,
    educationList: state.educationList.map((item, i) => 
      i === index ? { ...item, ...education } : item
    )
  })),
  
  on(EmployeeDocActions.deleteEducation, (state, { index }) => ({
    ...state,
    educationList: state.educationList.filter((_, i) => i !== index)
  })),
  
  // Work Experience actions
  on(EmployeeDocActions.addWorkExperience, (state, { experience }) => ({
    ...state,
    workExperienceList: [...state.workExperienceList, experience]
  })),
  
  on(EmployeeDocActions.updateWorkExperience, (state, { index, experience }) => ({
    ...state,
    workExperienceList: state.workExperienceList.map((item, i) => 
      i === index ? { ...item, ...experience } : item
    )
  })),
  
  on(EmployeeDocActions.deleteWorkExperience, (state, { index }) => ({
    ...state,
    workExperienceList: state.workExperienceList.filter((_, i) => i !== index)
  })),
  
  // Reset form state
  on(EmployeeDocActions.resetEmployeeForm, () => ({
    ...initialState
  })),
  
  // Delete Employee
  on(EmployeeDocActions.deleteEmployee, (state) => ({
    ...state,
    loading: true,
    error: null
  })),
  on(EmployeeDocActions.deleteEmployeeSuccess, (state, { id }) => ({
    ...state,
    employees: state.employees.filter(e => e.id !== id),
    loading: false
  })),
  on(EmployeeDocActions.deleteEmployeeFailure, (state, { error }) => ({
    ...state,
    error,
    loading: false
  }))
);


