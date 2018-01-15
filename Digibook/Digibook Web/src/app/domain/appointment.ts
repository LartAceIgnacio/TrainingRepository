import { Contact } from "./contact";

export interface Appointment {
    appointmentId?;
    appointmentDate?;
    guestId?;
    guestName?;
    hostId?;
    hostName?;
    startTime?;
    endTime?;
    isCancelled?;
    isDone?;
    notes?;
    contact?: Contact;
}