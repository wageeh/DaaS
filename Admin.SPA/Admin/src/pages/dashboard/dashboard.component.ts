import { Component, OnInit } from '@angular/core';
import { DoctorsService, Doctor } from '../../providers/doctors/doctors.service';
import { PatientsService, Patient } from '../../providers/patients/patients.service';
import { AppointmentsService, Appointment } from '../../providers/appointments/appointments.service';



interface AppointmentDetails extends Appointment{  
  Doctor:Doctor;
  Patient:Patient;
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  public PatientList:any;
  public DoctorList:any;
  public AppointmentList: any;
  public Result:AppointmentDetails[];
  public TriggerTime:string;

  constructor(private doctorsService:DoctorsService,
    private patientsService:PatientsService,
    private appointmentsService:AppointmentsService) { 
      
  }

  ngOnInit() {

    this.initNewCall();

    /*setInterval( ()=>{
      this.initNewCall();
    }, (1000 * 60));*/
    
  }

  public async initNewCall() {
    this.TriggerTime = new Date().toLocaleString();
    this.PatientList = await this.patientsService.ListPatients();
    this.DoctorList = await this.doctorsService.ListDoctors();
    this.AppointmentList = await this.appointmentsService.ListAppointments();
    
    this.fillResultList();
  }

  public fillResultList(){
    this.Result = [];
    var appointment:Appointment;
    if(this.AppointmentList != undefined){
    this.AppointmentList.forEach(appointment => {
      
      var appitem = {} as AppointmentDetails;
      appitem.AppointmentDate = appointment.AppointmentDate;
      appitem.CreatedDate = appointment.CreatedDate;
      appitem.appointmentTimeSlot = appointment.appointmentTimeSlot;
      appitem.HasConflict = appointment.HasConflict;
      appitem.DoctorId = appointment.DoctorId;
      appitem.Doctor = this.DoctorList.filter(x => x.itemId == appitem.DoctorId)[0];
      appitem.PatientId = appointment.PatientId;
      appitem.Patient = this.PatientList.filter(x => x.itemId == appitem.PatientId)[0];
      
      this.Result.push(appitem);
    });
  }
  }

  public findFromStatus(key:string){
    /*if(this.VehicleStatus!= undefined){
        var found = this.VehicleStatus.find(item=>item.VehId == key);
        if(found!=null)
        {
          return found.SentTime;
        }
    }*/
    return '';
  }

  public async customerNameChanged(event: any)
  {
    //this.filteredName = event.target.value;
    this.initNewCall();
  }

  public selectDoctorOnChange(target:any){
    this.initNewCall();
  }

  public selectPatientOnChange(target:any){
    this.initNewCall();
  }

}
