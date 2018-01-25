import {Venue} from '../domain/Venue';

export class VenueClass implements Venue{
    constructor(
        public venueId?,
        public venueName?,
        public description?
    ){}
}