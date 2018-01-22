import { Appointment } from '../appointments/appointment';

export class AppointmentClass implements Appointment {
    constructor(
        public appointmentDate?,
        public guestId?,
        public hostId?,
        public startTime?,
        public endTime?,
        public isCancelled?,
        public isDone?,
        public notes?,
        public guestName?,
        public hostName?,
        public appointmentId?
    ) { }
}