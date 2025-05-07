import * as fromEmployeeDoc from '../reducers/employee-doc.reducer';
import { selectEmployeeDocState } from './employee-doc.selectors';

describe('EmployeeDoc Selectors', () => {
  it('should select the feature state', () => {
    const result = selectEmployeeDocState({
      [fromEmployeeDoc.employeeDocFeatureKey]: {}
    });

    expect(result).toEqual({});
  });
});
