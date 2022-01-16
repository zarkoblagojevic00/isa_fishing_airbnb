<template>
    <section id="boxes">
        <div class="welcome">Welcome {{ name }}</div>
        <h1>Choose your next adventure</h1>
        <div class="container">
            <div class="box" @click="navigate({ name: 'BoatsExpo' })">
                <div id="boat-img-title-box" class="img-title-box">
                    <h3 class="img-title">Boats</h3>
                </div>
                <p>
                    Lorem, ipsum dolor sit amet consectetur adipisicing elit. Ut
                    accusamus blanditiis fugiat, quas delectus voluptates amet
                    consectetur placeat mollitia, quae dolorum.
                </p>
            </div>
            <div class="box" @click="navigate({ name: 'VillasExpo' })">
                <div id="villa-img-title-box" class="img-title-box">
                    <h3 class="img-title">Villas</h3>
                </div>
                <p>
                    Lorem, ipsum dolor sit amet consectetur adipisicing elit. Ut
                    accusamus blanditiis fugiat, quas delectus voluptates amet
                    consectetur placeat mollitia, quae dolorum.
                </p>
            </div>
            <div class="box" @click="navigate({ name: 'AdventuresExpo' })">
                <div id="adventure-img-title-box" class="img-title-box">
                    <h3 class="img-title">Adventures</h3>
                </div>
                <p>
                    Lorem, ipsum dolor sit amet consectetur adipisicing elit. Ut
                    accusamus blanditiis fugiat, quas delectus voluptates amet
                    consectetur placeat mollitia, quae dolorum.
                </p>
            </div>
        </div>
    </section>
</template>

<script>
import { getName } from "../utils/local-storage-util.js";
export default {
    name: "ClientWelcome",
    data() {
        return {
            name: getName(),
        };
    },
};
</script>

<style scoped>
.welcome {
    width: 81%;
    margin: 0 auto 50px auto;
    background: #aca;
    font-size: 2.5em;
    color: fafafa;
    display: flex;
    justify-content: flex-start;
    align-items: flex-start;
    padding: 1em;
    border-radius: 5px;
}
/* Boxes */
#boxes {
    padding-top: 20px;
}

.container {
    width: 90%;
    margin: auto;
    padding-bottom: 30px;
    display: flex;
    justify-content: space-around;
    align-items: center;
}

#boxes .box {
    padding: 10px;
    margin: 20px;
    text-align: center;
    border-radius: 5px;
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.6);
    cursor: pointer;
    transition: all 0.2s ease-in-out;
}

#boxes .box:hover {
    box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.8);
}

.img-title {
    position: absolute;
    margin: 0;
    padding: 60px 0 30px 0;
    width: 100%;
    color: #eee;
    background: linear-gradient(to top, black, transparent);
    border-radius: 5px;
}

.img-title-box {
    position: relative;
    min-height: 280px;
    display: flex;
    justify-content: center;
    align-items: flex-end;
    border-radius: 5px;
    background-size: cover;
    background-position: center;
}

#boat-img-title-box {
    background-image: url("../assets/boat-box.jpg");
}

#villa-img-title-box {
    background-image: url("../assets/villa-box.jpg");
}

#adventure-img-title-box {
    background-image: url("../assets/fishing.png");
}
</style>
