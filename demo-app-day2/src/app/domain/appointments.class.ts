import { IAppointments } from './IAppointments';

export class AppointmentsClass implements IAppointments {

    constructor(
        public appointmentId?,
        public appointmentDate?, 
        public guestId?,
        public guestName?,
        public hostId?,
        public hostName?,
        public startTime?,
        public endTime?,
        public isCancelled?,
        public isDone?,
        public notes?,
    ) {
        
    }
}