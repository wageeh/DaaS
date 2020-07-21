import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

interface Patient {
  email: string;
  name: string;
  description: string;
  address: {
    zipCode: string;
    entityId: string;
    itemId: string;
    createdDate: string;
  },
  entityId: string;
  itemId: string;
  createdDate: string;
}

interface Doctor {
  entityId: string;
  itemId: string;
  name: boolean;
  speciality: Speciality;
  description: string;
}

interface Appointment {
  PatientId: string;
  DoctorId: string;
  AppointmentDate: string;
  appointmentTimeSlot: number;
  HasConflict: Boolean;
  CreatedDate: string;
  id: string;
}

interface AppointmentDetails extends Appointment {
  Doctor: Doctor;
  Patient: Patient;
}

interface Speciality {
  id: string;
  name: string;
}
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})

export class HomeComponent {

  public PatientList: any;
  public DoctorList: any;
  public AppointmentList: any;
  public Result: AppointmentDetails[];
  public TriggerTime: string;
  public AppointmentListNew: [];
  public groupByName = {};

  public selecteddoctorid = "";
  public selectedpatientid = "";
  public selectedfromdate = "2020-07-17";
  public selectedtodate = "2020-07-22";


  public http: HttpClient;

  usermanagementurl: string = "https://daas-usermanagementapi.azurewebsites.net/api/";
  appointmenturl: string = "https://daas-appointmentsapi.azurewebsites.net/api/";

  ListAppointments(from: string, to: string, doctorid: string, patientid: string) {

    var querystr = "";
    var paramadded = false;
    if (from != "") {
      querystr += "from=" + from;
      paramadded = true;
    }
    if (to != "") {
      if (paramadded) {
        querystr += "&";
      }
      querystr += "to=" + to;
      paramadded = true;
    }
    if (doctorid != "") {
      if (paramadded) {
        querystr += "&";
      }
      querystr += "doctorid='" + doctorid + "'";
      paramadded = true;
    }
    if (patientid != "") {
      if (paramadded) {
        querystr += "&";
      }
      querystr += "patientid=" + patientid + "'";
    }

    this.http.get<any>(this.appointmenturl + "appointment?" + querystr).subscribe(result => {
      this.AppointmentList = result;
      this.fillResultList();
    }, error => console.error(error));
  }

  ListDoctors() {
    this.http.get<any>(this.usermanagementurl + "doctor/").subscribe(result => {
      this.DoctorList = result;
    }, error => console.error(error));
  }
  ListPatients() {
    this.http.get<any>(this.usermanagementurl + "patient/").subscribe(result => {
      this.PatientList = result;
    }, error => console.error(error));
  }

  public async initNewCall() {
    this.TriggerTime = new Date().toLocaleString();
    this.ListPatients();
    this.ListDoctors();
    this.ListAppointments(this.selectedfromdate, this.selectedtodate, this.selecteddoctorid, this.selectedpatientid);
  }

  constructor(private _http: HttpClient) {
    this.http = _http;

    this.initNewCall();
  }

  public fillResultList() {
    this.Result = [];
    var appointment: Appointment;
    if (this.AppointmentList != undefined) {
      this.AppointmentList.forEach(appointment => {

        var appitem = {} as AppointmentDetails;
        appitem.AppointmentDate = appointment.appointmentDate;
        appitem.CreatedDate = appointment.createdDate;
        appitem.appointmentTimeSlot = appointment.appointmentTimeSlot;
        appitem.HasConflict = appointment.hasConflict;
        appitem.DoctorId = appointment.doctorId;
        appitem.Doctor = this.DoctorList.filter(x => x.itemId == appitem.DoctorId)[0];
        appitem.PatientId = appointment.patientId;
        appitem.Patient = this.PatientList.filter(x => x.itemId == appitem.PatientId)[0];
        this.Result.push(appitem);
      });
    }

  }

  public selectDoctorOnChange(target: any) {
    this.selecteddoctorid = target.value;
    this.initNewCall();
  }

  public selectPatientOnChange(target: any) {
    this.selectedpatientid = target.value;
    this.initNewCall();
  }

  public selectfromChange(target: any) {
    this.selectedfromdate = target.value;
    this.initNewCall();
  }

  public selecttoChange(target: any) {
    this.selectedtodate = target.value;
    this.initNewCall();
  }
}

