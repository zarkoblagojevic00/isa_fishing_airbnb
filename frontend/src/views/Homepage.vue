<template>
    <!-- <div>
        <h1>TODO: Homepage</h1>
        <div>
            <router-link to="adventure">
                Adventure profile (temporary)
            </router-link>
        </div>
        <div>
            <router-link to="instructor">
                instructor homepage (temporary)
            </router-link>
        </div>
        <div>
            <router-link to="login"> Login </router-link>
        </div>
        <div>
            <router-link to="TestPage"> TestPage </router-link>
        </div>
        <div>
            <router-link to="admin"> Admin </router-link>
        </div>
    </div> -->

    <section id="amen">
        <h2>Who are we?</h2>
        <div>
            Lorem ipsum dolor sit, amet consectetur adipisicing elit. Sit velit
            non ullam recusandae, soluta quaerat fuga, expedita harum at fugit
            ipsa eum quasi laboriosam alias temporibus id exercitationem eius
            quisquam aliquam? Dolores hic veritatis deserunt praesentium
            necessitatibus totam omnis similique inventore, quasi ex sunt vel
            vitae iste corporis laborum sit.
        </div>

        <h2>What do we do?</h2>
        <div>
            Lorem ipsum dolor sit, amet consectetur adipisicing elit. Sit velit
            non ullam recusandae, soluta quaerat fuga, expedita harum at fugit
            ipsa eum quasi laboriosam alias temporibus id exercitationem eius
            quisquam aliquam? Dolores hic veritatis deserunt praesentium
            necessitatibus totam omnis similique inventore, quasi ex sunt vel
            vitae iste corporis laborum sit.
        </div>

        <h2>Why us?</h2>
        <div>
            Lorem ipsum dolor sit, amet consectetur adipisicing elit. Sit velit
            non ullam recusandae, soluta quaerat fuga, expedita harum at fugit
            ipsa eum quasi laboriosam alias temporibus id exercitationem eius
            quisquam aliquam? Dolores hic veritatis deserunt praesentium
            necessitatibus totam omnis similique inventore, quasi ex sunt vel
            vitae iste corporis laborum sit.
        </div>
    </section>

    <div id="lake-image">
        <h1>Explore with us</h1>
    </div>

    <section id="boxes">
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

    <!--<v-date-picker v-model="date" isRange="true" mode="dateTime" />-->
</template>

<script>
export default {
    name: "Homepage",

    methods: {
        navigate(route) {
            this.$router.push(route);
        },
    },
    data() {
        return {
            ids: [0, 1],
        };
    },
};
</script>

<style scoped>
/* Boxes */
#boxes {
    padding-top: 20px;
    background-color: #ada;
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
    background-color: #aadfaa;
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
    min-height: 320px;
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

#amen {
    font-size: 1.2rem;
    padding: 50px 5% 80px 5%;
}

#lake-image {
    width: 100%;
    height: 450px;

    display: flex;
    justify-content: center;
    align-items: center;

    color: rgba(255, 255, 255, 0.8);
    font-family: "Brush Script MT", cursive;
    font-size: 2.5rem;

    background: linear-gradient(rgba(0, 0, 0, 0.85), transparent),
        url("../assets/homepage-lake-1.jpeg");
    background-size: cover;
    background-position: center;
    background-attachment: fixed;
}
</style>
