<template>
    <ol-map class="display-map">
        <ol-view
            ref="view"
            :rotation="rotation"
            :projection="projection"
            :center="center"
            :zoom="16"
            :minZoom="8"
            :maxZoom="18"
        />

        <ol-tile-layer>
            <ol-source-osm />
        </ol-tile-layer>
        <ol-vector-layer>
            <ol-source-vector>
                <ol-feature>
                    <ol-geom-point :coordinates="coordinates"></ol-geom-point>
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
</template>

<script>
import { ref } from "vue";
import mapMarker from "../assets/map-marker.png";
import { fromLonLat } from "ol/proj";
export default {
    props: {
        lon: {
            type: Number,
            required: true,
        },
        lat: {
            type: Number,
            required: true,
        },
    },
    setup(props) {
        const projection = ref("EPSG:3857");
        const rotation = ref(0);

        const view = ref(null);
        const map = ref(null);
        const markerIcon = mapMarker;
        const coordinates = fromLonLat([props.lon, props.lat]);
        const center = coordinates;

        return {
            projection,
            rotation,
            view,
            map,
            markerIcon,
            coordinates,
            center,
        };
    },
};
</script>

<style scoped>
.display-map {
    height: 36vh;
    width: 100%;
}
</style>
