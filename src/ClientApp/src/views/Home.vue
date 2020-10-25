<template>

  <div class="home">
    
    <div v-for="ctrl of switches" :key="ctrl.name">
        <v-btn @click="toggle(ctrl.putGroup, ctrl.putOffset)">{{ ctrl.name }}: {{ state[ctrl.name] }}</v-btn>
    </div>

    <v-btn @click="refresh">REFRESH</v-btn>
  </div>
</template>

<script lang="ts">

import { Component, Vue } from 'vue-property-decorator';
import { ValuesService } from '../services/values-service';



@Component({

})
export default class Home extends Vue {

    public switches = [
        {
            name: 'Bad',
            putGroup: 0x4021,
            putOffset: 0x00,
            getGroup: 0x4021,
            getOffset: 0xA0
        },
        {
            name: 'Schlafzimmer',
            putGroup: 0x4021,
            putOffset: 0x01,
            getGroup: 0x4021,
            getOffset: 0xA1
        },
        {
            name: 'Wintergarten',
            putGroup: 0x4021,
            putOffset: 0x02,
            getGroup: 0x4021,
            getOffset: 0xA2
        },
        {
            name: 'Waschküche',
            putGroup: 0x4021,
            putOffset: 0x03,
            getGroup: 0x4021,
            getOffset: 0xA3
        },
        {
            name: 'Aussenbereich',
            putGroup: 0x4021,
            putOffset: 0x04,
            getGroup: 0x4021,
            getOffset: 0xA4
        },
        {
            name: 'Gäste WC',
            putGroup: 0x4021,
            putOffset: 0x08,
            getGroup: 0x4021,
            getOffset: 0xA8
        },
        {
            name: 'Wohnbereich',
            putGroup: 0x4021,
            putOffset: 0x09,
            getGroup: 0x4021,
            getOffset: 0xA9
            },
            {
            name: 'Essbereich',
            putGroup: 0x4021,
            putOffset: 0x0A,
            getGroup: 0x4021,
            getOffset: 0xAA
        },
        {
            name: 'Küche',
            putGroup: 0x4021,
            putOffset: 0x0B,
            getGroup: 0x4021,
            getOffset: 0xAB
        },
        {
            name: 'Flur',
            putGroup: 0x4021,
            putOffset: 0x0C,
            getGroup: 0x4021,
            getOffset: 0xAC
        },
        {
            name: 'Abstellraum',
            putGroup: 0x4021,
            putOffset: 0x0D,
            getGroup: 0x4021,
            getOffset: 0xAD
        },
    ];

    state: { [name: string]: boolean | null } = {};

    constructor() {
        super();

        this.state = {
            'Esszimmer OG': null,
            'Esszimmer UG': null,
            '...': null,
        };
    }

    mounted() {
        this.refresh();
    }

    async refresh() {
        for (const ctrl of this.switches) {
            const value = await ValuesService.get(ctrl.getGroup, ctrl.getOffset);
            this.state[ctrl.name] = value;
        }
    }

    async toggle(group: number, offset: number) {
        await ValuesService.put(group, offset, true);
        await ValuesService.put(group, offset, false);
    }
}

</script>
