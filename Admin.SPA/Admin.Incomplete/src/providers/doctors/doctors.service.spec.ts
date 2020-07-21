import { TestBed, async, inject } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { DoctorsService } from './doctors.service';

describe('DoctorsService', () => {
  let doctorsService: DoctorsService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule,
      ],
      providers: [
        DoctorsService
      ],
    });

    doctorsService = TestBed.get(DoctorsService);
    httpMock = TestBed.get(HttpTestingController);
  });

  it(`should fetch the basic doctors data`, async( async () => {             
    var customerlistobject = await doctorsService.ListDoctorData('');
    expect(customerlistobject).toBeGreaterThan(3);    
  }));
  
});