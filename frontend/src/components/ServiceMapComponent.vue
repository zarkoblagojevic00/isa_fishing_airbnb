<template>
    <div class="abs-div">
        <div class="whole-map-div">
            <div class="map-wrapper">
                <div class="map-div">
                    <ol-map class="map" @click="clicked">
                        <ol-view
                            ref="view"
                            :center="center"
                            :rotation="rotation"
                            :zoom="zoom"
                            :projection="projection"
                        />

                        <ol-tile-layer>
                            <ol-source-osm />
                        </ol-tile-layer>
                        <ol-vector-layer>
                            <ol-source-vector>
                                <ol-feature>
                                    <ol-geom-point
                                        :coordinates="coordinate"
                                    ></ol-geom-point>
                                    <ol-style>
                                        <ol-style-icon
                                            :src="markerIcon"
                                            :scale="1"
                                        ></ol-style-icon>
                                    </ol-style>
                                </ol-feature>
                            </ol-source-vector>
                        </ol-vector-layer>
                    </ol-map>
                    <div class="input-wrapper">
                        <span class="label">Longitude:</span>
                        <input
                            class="input-field"
                            type="text"
                            v-model="longitude"
                            disabled
                        />
                    </div>
                    <div class="input-wrapper">
                        <span class="label">Latitude:</span>
                        <input
                            class="input-field"
                            type="text"
                            v-model="latitude"
                            disabled
                        />
                    </div>
                    <div class="submit-div">
                        <button class="submit-btn" @click="Toggle()">
                            Return
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { ref } from "vue";
import markerIcon from "@/assets/address.png";
export default {
    name: "ServiceMap",
    setup() {
        const projection = ref("EPSG:4326");
        const zoom = ref(8);
        const rotation = ref(0);

        const view = ref(null);
        const map = ref(null);

        return {
            projection,
            zoom,
            rotation,
            view,
            map,
            markerIcon,
        };
    },
    props: {
        coordinates: Array,
        updateShowMap: Function,
        updateCoords: Function,
    },
    components: {},
    data() {
        return {
            center:
                this.$props.coordinates != undefined
                    ? this.$props.coordinates
                    : [19.833549, 45.267136],
            coordinate: this.$props.coordinates,
            longitude:
                this.$props.coordinates != undefined
                    ? this.$props.coordinates[0]
                    : "",
            latitude:
                this.$props.coordinates != undefined
                    ? this.$props.coordinates[1]
                    : "",
            buttonText: this.$props.prevPage,
        };
    },
    mounted() {},
    methods: {
        clicked(evt) {
            if (evt.isTrusted != undefined) {
                return;
            }
            this.coordinate = evt.coordinate;
            this.longitude = evt.coordinate[0];
            this.latitude = evt.coordinate[1];
            this.$props.updateCoords(this.longitude, this.latitude);
        },
        Toggle() {
            this.$props.updateShowMap();
        },
    },
};
</script>

<style scoped>
.abs-div {
    position: absolute;
    height: 100%;
    width: 100%;
    top: 0;
}
.whole-map-div {
    width: 100%;
    height: 100%;
    top: 0;
    left: 0;
    background-color: rgba(0, 0, 0, 0.5);
}
.map-wrapper {
    position: sticky;
    height: 100vh;
    top: 0px;
}

.map-div {
    position: relative;
    top: 100px;
}

.map {
    position: relative;
    margin: auto;
    width: 500px;
    height: 500px;
    margin-bottom: 15px;
}

.input-wrapper {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    margin-bottom: 15px;
}

.label {
    font-size: 14px;
    color: white;
    margin-bottom: 5px;
    padding-left: 10px;
    margin-left: auto;
    margin-right: auto;
    width: auto !important;
}

.input-field {
    height: 50px;
    border-radius: 15px;
    outline: none;
    border: 1px solid #c3c3c3;
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    padding-left: 15px;
    padding-right: 15px;
    box-sizing: border-box;
    font-size: 15px;
    margin-left: auto;
    margin-right: auto;
    background-color: white;
}

.submit-btn {
    height: 50px;
    border-radius: 15px;
    border: none;
    font-size: 20px;
    color: white;
    background-color: #345fed;
    cursor: pointer;
    font-size: 18px;
    transition: background-color 300ms linear;
}

.submit-btn:hover {
    background-color: #54cc39;
    color: black;
}
</style>
