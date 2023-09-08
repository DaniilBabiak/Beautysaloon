export interface ReservationModel {
  id: number;
  serviceId: number;
  serviceName: string;
  dateTime: Date;
  customerPhoneNumber: string;
  masterId: number;
  masterName: string;
}