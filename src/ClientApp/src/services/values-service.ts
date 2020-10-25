import axios from 'axios';

const BASE_URL: string = process.env.VUE_APP_BASE_URL;

export class ValuesService {

    public static async get(group: number, offset: number): Promise<boolean> {
        const response = await axios.get<boolean>(`${BASE_URL}api/groups/${group}/offsets/${offset}`);

        return response.data;
    }

}

