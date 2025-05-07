// selectors/dashboard.selectors.ts
import { createFeatureSelector, createSelector } from '@ngrx/store';
import { EmployeeState } from '../reducers/employee-doc.reducer';

export const selectEmployeeState = createFeatureSelector<EmployeeState>('employee');

export const selectEmployees = createSelector(
  selectEmployeeState,
  state => state.employees
);

export const selectEmployeeFullState = createSelector(
  selectEmployeeState,
  state => state
);

export const selectCurrentEmployee = createSelector(
  selectEmployeeState,
  (state: EmployeeState) => state.currentEmployee
);

export const selectEmergencyContacts = createSelector(
  selectEmployeeState,
  (state: EmployeeState) => state.emergencyContacts
);

export const selectEducationList = createSelector(
  selectEmployeeState,
  (state: EmployeeState) => state.educationList
);

export const selectWorkExperienceList = createSelector(
  selectEmployeeState,
  (state: EmployeeState) => state.workExperienceList
);
export const selectSaveSuccess = createSelector(
  selectEmployeeState,
  (state: EmployeeState) => state.saveSuccess
);
export const selectLoading = createSelector(
  selectEmployeeState,
  state => state.loading
);

export const selectError = createSelector(
  selectEmployeeState,
  state => state.error
);

export const selectSearchRequest = createSelector(
  selectEmployeeState,
  state => state.searchRequest
);

export const selectListResponse = createSelector(
  selectEmployeeState,
  state => state.listResponse
);
export const selectEmployeeFormData = createSelector(
  selectCurrentEmployee,
  selectEmergencyContacts,
  selectEducationList,
  selectWorkExperienceList,
  (employee, emergencyContacts, educationList, workExperienceList) => ({
    employee,
    emergencyContacts,
    educationList,
    workExperienceList
  })
);