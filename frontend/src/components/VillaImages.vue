<template>
    <div class="add-img-div">
        <h4>Add images here:</h4>
        <div class="drop-div" :class="images.length == 0 ? 'drop-img' : ''">
            <input
                type="file"
                class="file"
                aria-hidden="true"
                multiple="multiple"
                accept="image/*"
                ref="myInput"
                @change="ChangeImages()"
            />
            <h5 class="drop-hint" v-if="images.length == 0">Drop pics here</h5>

            <div class="image-label" v-for="image in images" :key="image.name">
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
    <div class="wrapperdiv">
        <div class="singleimage" v-for="img in existingImages" :key="img">
            <button class="delbtn" @click="DeleteImage(img)"></button>
            <img :src="'/api/Image/GetImage?id=' + img" />
        </div>
    </div>
</template>

<script>
import swalCommons from "../mixins/swal-commons.js";
export default {
    name: "VillaImages",
    mixins: [swalCommons],
    props: {
        changeMode: Function,
        villaId: Number,
    },
    data() {
        return {
            images: [],
            existingImages: [],
        };
    },
    mounted() {
        this.PullData();
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
            if (this.mode == "Adding") return;

            let vue = this;
            fetch(
                "/api/VillaManagement/GetVillaInfo?villaId=" +
                    this.$props.villaId,
                {
                    method: "GET",
                    redirect: "follow",
                    headers: {
                        "Content-type": "application/json",
                        "Set-Cookie": document.cookie,
                    },
                }
            )
                .then((response) => {
                    if (response.status != 200) {
                        vue.toast.fire({
                            icon: "error",
                            title:
                                "Something went wrong!\nStatus code: " +
                                response.status,
                        });
                        throw "";
                    }
                    return response.json();
                })
                .then((data) => {
                    vue.existingImages = data.imageIds;
                });
        },
        DeleteImage(imageId) {
            let vue = this;
            fetch("/api/Image/DeleteImage?id=" + imageId, {
                method: "DELETE",
                redirect: "follow",
                headers: {
                    "Content-type": "application/json",
                    "Set-Cookie": document.cookie,
                },
            }).then((response) => {
                if (response.status == 200) {
                    vue.PullData();
                } else {
                    vue.toast.fire({
                        icon: "error",
                        title:
                            "Something went wrong!\nStatus code: " +
                            response.status,
                    });
                }
            });
        },
        async SendImages() {
            let newImages = new Array();
            for (let img of this.images) {
                let temp = await this.GetBase64(img);
                newImages.push({
                    serviceId: this.$props.villaId,
                    imageAsBase64: temp,
                });
            }
            let vue = this;
            for (let img of newImages) {
                await fetch("/api/Image/AddImage", {
                    method: "POST",
                    redirect: "follow",
                    headers: {
                        "Content-type": "application/json",
                        "Set-Cookie": document.cookie,
                    },
                    body: JSON.stringify(img),
                }).then((response) => {
                    if (response.status != 200) {
                        vue.toast.fire({
                            icon: "error",
                            title:
                                "Something went wrong!\nStatus code: " +
                                response.status,
                        });
                    }
                });
            }

            this.PullData();
            this.images = new Array();
        },
    },
};
</script>

<style scoped>
.wrapperdiv {
    height: 100%;
    width: 100%;
    display: flex;
    flex-flow: row wrap;
    position: relative;
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
    height: 350px;
    align-items: center;
    margin-bottom: 5px;
    display: flex;
    flex-direction: column;
    display: relative;
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
