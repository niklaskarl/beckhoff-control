<template>
    <v-app>
        <v-app-bar app color="primary" dark>
            <v-icon>mdi-power</v-icon>
            <v-toolbar-title class="ml-3">Steuerung</v-toolbar-title>

            <v-spacer />

            <v-btn icon @click="refresh()">
                <v-icon>mdi-refresh</v-icon>
            </v-btn>
        </v-app-bar>

        <v-main>
            <v-expansion-panels multiple tile :value="[ 0, 1 ]">
                <v-expansion-panel>
                    <v-expansion-panel-header>Licht</v-expansion-panel-header>
                    <v-expansion-panel-content>
                        <v-container fluid>
                            <v-row v-if="!items" justify="center" class="mt-4">
                                <v-progress-circular indeterminate>
                                </v-progress-circular>
                            </v-row>
                            <v-row>
                                <v-col v-for="item of items" :key="item.light.id" lg="3" md="4" sm="6" cols="12" class="flex-grow-0 flex-shrink-0">
                                    <v-card tile>
                                        <v-card-title>
                                            <v-icon large>
                                                {{ item.light.icon }}
                                            </v-icon>
                                            <v-text large>
                                                {{ item.light.name }}
                                            </v-text>
                                        </v-card-title>
                                    </v-card>
                                </v-col>     
                                <v-spacer></v-spacer>
                                <v-col>
                                    <span style="font-weight: bold;" :style="{ color: (item.power && item.power.value) ? 'green' : 'red' }">
                                                    {{ (item.power && item.power.value) ? 'AN' : 'AUS' }} </span> 
                                </v-col>
                                         
                                <v-col> 
                                    <v-card>       
                                        <v-card-text>
                                            <v-icon x-large>mdi-lightbulb</v-icon>
                                            <span style="font-weight: bold;" :style="{ color: (item.power && item.power.value) ? 'green' : 'red' }">{{ (item.power && item.power.value) ? 'AN' : 'AUS' }}</span>
                                        </v-card-text>
                                        <v-card-actions>
                                            <v-btn style="width: 100%" tile @click="toggle(item)">{{ (item.power && item.power.value) ? 'AUSSCHALTEN' : 'EINSCHALTEN' }}</v-btn>
                                        </v-card-actions> 
                                    </v-card>
                                </v-col>  
                            </v-row>                               
                        </v-container>
                    </v-expansion-panel-content>
                </v-expansion-panel>
                <v-expansion-panel>
                    <v-expansion-panel-header>Rollläden</v-expansion-panel-header>
                    <v-expansion-panel-content>
                        ...
                    </v-expansion-panel-content>
                </v-expansion-panel>
                <v-expansion-panel>
                    <v-expansion-panel-header>Sonstiges</v-expansion-panel-header>
                    <v-expansion-panel-content>

                        <v-icon x-large>mdi-cup</v-icon>
                                                            <v-card>
                                                                    <v-toolbar flat>
                                                                    <v-toolbar-title>TEST </v-toolbar-title>
                                                                    <v-spacer></v-spacer>
                                                                        <div>
                                                                            <v-switch
                                                                                v-model="sticky"
                                                                                label="Sticky Banner"
                                                                                hide-details
                                                                                ></v-switch>
                                                                        </div>
                                                                    </v-toolbar>
                                                                    <v-banner single-line:sticky="sticky">                                                              
                                                                        We can't save your edits while you are in offline mode.
                                                                        <v-btn 
                                                                            text
                                                                            color="deep-purple accent-4"
                                                                            >
                                                                            Get Online
                                                                        </v-btn>
                                                                    </v-banner>
                                                                    <v-card-text class="grey lighten-4">
                                                                        <v-sheet
                                                                            max-width="800"
                                                                            height="300"
                                                                            class="mx-auto"
                                                                        ></v-sheet>
                                                                    </v-card-text>
                                                            </v-card>

                    </v-expansion-panel-content>
                </v-expansion-panel>
            </v-expansion-panels>
        </v-main>

        <v-footer app absolute>
            <v-spacer />
            <router-link class="footer-link" to="/about">Impressum</router-link>
        </v-footer>
    </v-app>
</template>

<script lang="ts">

import { Component, Vue } from 'vue-property-decorator';
import { LightModel, LightPowerModel } from '../models/light.model';
import * as LightService from '../services/light.service';

interface Item {
    light: LightModel;
    power: LightPowerModel | null;
}

@Component({

})
export default class Home extends Vue {

    private interval: number | null = null;

    private items: Item[] | null = null;

    constructor() {
        super();
    }

    async mounted() {

        this.items = (await LightService.getLights()).map<Item>(l => ({ light: l, power: null }));
        await this.refresh();

        /*this.interval = setInterval(() => {
            this.refresh();
        }, 1000);*/
    }

    unmounted() {
        if (this.interval != null) {
            clearInterval(this.interval);
            this.interval = null;
        }
    }

    async refresh() {
        if (this.items) {
            for (const item of this.items) {
                item.power = await LightService.getPower(item.light.id);
            }
        }
    }

    async toggle(item: Item) {
        item.power = await LightService.putPower(item.light.id, !(item.power?.value ?? false))
    }
}

</script>

<style lang="scss" scoped>

</style>
