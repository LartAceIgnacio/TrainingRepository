import { Flight } from "./Flight";

export class FlightClass implements Flight {
    constructor(
        public flightId?,
        public cityOfOrigin?,
        public cityOfDestination?,
        public eta?,
        public etd?,
        public flightCode?,
        public dateCreated?,
        public dateModified?
    ){
        
    }
}