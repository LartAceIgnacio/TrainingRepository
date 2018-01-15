import { Appointment } from "../domain/appointment"

export class AppointmentClass implements Appointment {
    constructor (public appointmentId?, public appointmentDate?, public guestId?, public guestName?,
        public hostId?, public hostName?, public startTime?,
        public endTime?, public isCancelled?, public isDone?, public notes?, public contact?) 
    {
        
    }
}