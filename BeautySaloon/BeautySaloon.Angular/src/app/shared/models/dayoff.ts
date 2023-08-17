import { Master } from "./master";

export interface DayOff {
    id: number | null;
    masterId: number;
    master: Master | null;
    date: Date;
  }