import { Appointment } from './appointment';

export class AppointmentClass implements Appointment {
    constructor(public appointmentId?,
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
        public appointmentCount?) {}
}
