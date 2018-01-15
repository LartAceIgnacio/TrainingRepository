import {Venue} from '../domain/venue';
export class VenueClass implements Venue{

    constructor(public venueId?, public venueName?, public description?
    , public venueCount?){

        }
}