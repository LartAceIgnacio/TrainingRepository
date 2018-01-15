import { Venue } from '../venues/venue';
export class VenueClass implements Venue {

    constructor(public venueId?, public venueName?, public description?, venueCount?) { }
}