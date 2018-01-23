import { Flight } from '../domain/flight';
export class FlightClass implements Flight {

    constructor(public flightId?, public cityOfOrigin?, public cityOfDestination?, public expectedTimeOfArrival?,
        public flightCode?, public expectedTimeOfDeparture?) {

    }
}