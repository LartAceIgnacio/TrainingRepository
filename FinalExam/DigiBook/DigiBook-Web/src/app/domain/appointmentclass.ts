import { Appointment } from "./appointment";


 export class Appointmentclass implements Appointment{
    
        constructor(public appointmentId?, public guestId?, public guestName?, public hostId?, public hostName?, public startTime?, public endTime?, public isCancelled?
                    , public isDone?, public notes?, public appointmentDate?) { }   
    }