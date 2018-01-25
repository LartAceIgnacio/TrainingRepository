import { Reservation } from "./Reservation";

export class ReservationClass implements Reservation {

    constructor(
        public reservationId?,
        public venueName?,
        public description?,
        public startDate?,
        public endDate?
    ){

    }

}