import axios from 'axios';
import { LightModel, LightPowerModel } from '../models/light.model';

const BASE_URL: string = process.env.VUE_APP_BASE_URL;

export async function getLights(): Promise<LightModel[]> {
    const response = await axios.get<LightModel[]>(`${BASE_URL}api/lights`);
    if (response.status >= 400) {
        throw new Error(`Request failed with status HTTP ${response.status}: ${response.statusText}`);
    }

    return response.data;
}

export async function getPower(lightId: number): Promise<LightPowerModel> {
    const response = await axios.get<LightPowerModel>(`${BASE_URL}api/lights/${lightId}/power`);
    if (response.status >= 400) {
        throw new Error(`Request failed with status HTTP ${response.status}: ${response.statusText}`);
    }

    return response.data;
}

export async function putPower(lightId: number, value: boolean): Promise<LightPowerModel> {
    const body: LightPowerModel = {
        value: value
    };

    const response = await axios.put<LightPowerModel>(`${BASE_URL}api/lights/${lightId}/power`, body);
    if (response.status >= 400) {
        throw new Error(`Request failed with status HTTP ${response.status}: ${response.statusText}`);
    }

    return response.data;
}
