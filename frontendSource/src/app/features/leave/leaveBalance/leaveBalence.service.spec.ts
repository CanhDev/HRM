/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { LeaveBalenceService } from './leaveBalence.service';

describe('Service: LeaveBalence', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LeaveBalenceService]
    });
  });

  it('should ...', inject([LeaveBalenceService], (service: LeaveBalenceService) => {
    expect(service).toBeTruthy();
  }));
});
