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
            name: 'Esszimmer OG',
            putGroup: 0x4021,
            putOffset: 0x0A,
            getGroup: 0x4021,
            getOffset: 0xAA
        },
        {
            name: 'Esszimmer UG',
            putGroup: 0x4021,
            putOffset: 0x0A,
            getGroup: 0x4021,
            getOffset: 0xAA
        },
        {
            name: '...',
            putGroup: 0x4021,
            putOffset: 0x0A,
            getGroup: 0xF031,
            getOffset: 0x0A
        }
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
