<template>
    <div>
        <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
        <div class="images">
            <div class="flexbox-column">
                <h1>Images from previous adventures</h1>
                <BaseCarousel :imageIds="existingImages" />
                <div class="wrapperdiv">
                    <div
                        class="singleimage"
                        v-for="img in existingImages"
                        :key="img"
                    >
                        <button
                            class="delbtn"
                            @click="DeleteImage(img)"
                            v-if="currentRole == 'Instructor'"
                        ></button>
                        <img :style="getImageStyle(img)" />
                    </div>
                </div>
            </div>

            <div class="add-img-div" v-if="currentRole == 'Instructor'">
                <h4>Add images here:</h4>
                <div
                    class="drop-div"
                    :class="images.length == 0 ? 'drop-img' : ''"
                >
                    <input
                        type="file"
                        class="file"
                        aria-hidden="true"
                        multiple="multiple"
                        accept="image/*"
                        ref="myInput"
                        @change="ChangeImages()"
                    />
                    <h5 class="drop-hint" v-if="images.length == 0">
                        Drop pics here
                    </h5>

                    <div
                        class="image-label"
                        v-for="image in images"
                        :key="image.name"
                    >
                        {{ image.name }}
                    </div>
                </div>
                <button
                    class="add-img-btn"
                    v-if="images.length != 0"
                    @click="SendImages()"
                >
                    Send images
                </button>
            </div>
        </div>
    </div>
</template>
<script>
import Navbar from "@/components/Navbar.vue";
import BaseCarousel from "@/components/BaseCarousel.vue";
import fetchImageBackground from "../mixins/fetch-image-bg.js";
import axios from "../api/api.js";

export default {
    name: "Gallery",
    mixins: [fetchImageBackground],
    components: {
        Navbar,
        BaseCarousel,
    },
    mounted() {
        this.PullData();
        this.currentRole = localStorage.getItem("role");
    },
    data() {
        return {
            navbarItems: ["Home", "Quick reservation", "Gallery"],
            baseUrlInstructor: "/adventure/" + this.$route.params.id + "/",
            // images: [
            //     "gallery-item-1.jpg",
            //     "gallery-item-2.jpg",
            //     "gallery-item-3.jpg",
            //     "gallery-item-1.jpg",
            //     "gallery-item-2.jpg",
            //     "gallery-item-3.jpg",
            // ],
            images: [],
            existingImages: [],
            currentRole: "",
        };
    },
    methods: {
        ChangeImages() {
            this.images = this.$refs.myInput.files;
        },
        GetBase64(file) {
            return new Promise((resolve, reject) => {
                let reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = () => resolve(reader.result);
                reader.onerror = (error) => reject(error);
            });
        },
        PullData() {
            axios
                .get(
                    "/api/Adventure/GetAdventureInfoById?adventureId=" +
                        this.$route.params.id
                )

                .then((response) => {
                    if (response.status != 200) {
                        alert(
                            "Something went wrong!\nStatus code: " +
                                response.status
                        );
                        throw "";
                    }
                    this.existingImages = response.data.imageIds;
                    console.log("image ids: ", this.existingImages);
                });
        },
        DeleteImage(imageId) {
            axios.delete("/api/Image/DeleteImage?id=" + imageId).then(() => {
                this.PullData();
            });
        },
        async SendImages() {
            let newImages = new Array();
            for (let img of this.images) {
                let temp = await this.GetBase64(img);
                newImages.push({
                    serviceId: this.$route.params.id,
                    imageAsBase64: temp,
                });
            }

            for (let img of newImages) {
                await axios.post("/api/Image/AddImage", img);
            }

            this.PullData();
            this.images = new Array();
        },
    },
};
</script>

<style scoped>
.images {
    display: flex;
    flex-wrap: wrap;
    justify-content: space-between;
    padding: 50px;
}

.flexbox-column {
    display: flex;
    flex-direction: column;
    justify-content: flex-start;
}

p {
    font-size: 26px;
    font-weight: bolder;
}

.wrapperdiv {
    display: flex;
    flex-flow: row wrap;
}

.singleimage {
    max-width: 560px;
    box-sizing: border-box;
    max-height: 280px;
    display: flex;
    margin-left: 5px;
    margin-right: 5px;
    margin-bottom: 5px;
    height: auto;
    min-height: 140px;
    min-width: 280px;
    position: relative;
}

img {
    width: 100%;
    height: 100%;
    object-fit: cover;
}

.delbtn {
    position: absolute;
    height: 35px;
    width: 35px;
    outline: none;
    border: none;
    background-color: black;
    cursor: pointer;
    top: 0;
    right: 0;
    background-repeat: no-repeat;
    background-position: center;
    background-image: url("../assets/trash.png");
}

.add-img-div {
    height: 250px;
    align-items: center;
    margin-bottom: 5px;
    display: flex;
    flex-direction: column;
    display: relative;
    padding-right: 50px;
}

.drop-div {
    height: calc(100% - 120px);
    max-width: 450px;
    width: 100%;
    margin-bottom: 5px;
    border-radius: 25px;
    border: 2px dashed black;
    position: relative;
    background-repeat: no-repeat;
    background-position: center;
    overflow: auto;
}

.drop-div::-webkit-scrollbar {
    display: none;
}

.add-img-btn {
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

.add-img-btn:hover {
    background-color: #54cc39;
    color: black;
}

.file {
    width: 100%;
    height: 100%;
    left: 0;
    top: 0;
    position: absolute;
    opacity: 0;
    z-index: 1;
}

.file:hover {
    cursor: pointer;
}

.drop-hint {
    position: relative;
    top: calc(50% + 10px);
}

.drop-img {
    background-image: url("../assets/drop.png");
}

.image-label {
    height: 48px;
    border-bottom: 1px solid black;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: 15px;
}
</style>
