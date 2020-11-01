<template>
    <v-app>
        <v-app-bar app color="primary" dark>
            <v-icon>mdi-power</v-icon>
            <v-toolbar-title class="ml-3">Impressum</v-toolbar-title>

            <v-spacer />
        </v-app-bar>

        <v-main>
            <v-container>
                © Jonny Grummt, Niklas Karl 2020
            </v-container>
        </v-main>

        <v-footer app absolute>
            <v-spacer />
            <router-link class="footer-link" to="/about">Impressum</router-link>
        </v-footer>
    </v-app>
</template>

<script lang="ts">

import { Component, Vue } from 'vue-property-decorator';

@Component({

})
export default class About extends Vue {
    constructor() {
        super();
    }
}

</script>

<style lang="scss" scoped>

</style>
