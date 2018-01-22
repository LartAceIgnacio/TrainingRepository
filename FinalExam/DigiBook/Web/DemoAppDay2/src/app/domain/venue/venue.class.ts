import { Venue } from '../venue/venue';
export class VenueClass implements Venue {
    constructor(
        public venueId?,
        public venueName?,
        public description?
    ) { }
}