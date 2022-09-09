let scene3D;
let camera;
let renderer;
let vertices;
let indices;

function createScene() {
    const container = document.getElementById('mesh-renderer');
    const tags = container.getElementsByTagName("canvas");

    if (tags.length > 0) {
        container.removeChild(tags[0]);
    }

    scene3D = new THREE.Scene();
    camera = new THREE.PerspectiveCamera(75, 1, 0.1, 1000);

    renderer = new THREE.WebGLRenderer();
    renderer.setSize(256, 256);
    container.appendChild(renderer.domElement);
}

function clearScene() {
    while (scene3D.children.length > 0) {
        scene3D.remove(scene3D.children[0]);
    }
}

function sendVerts(verts) {
    var m = verts + 12;
    var r = Module.HEAP32[m >> 2];
    vertices = new Float32Array(Module.HEAPF32.buffer, m + 4, r);
}

function sendInds(inds) {
    var m = inds + 12;
    var r = Module.HEAP32[m >> 2];
    indices = new Int32Array(Module.HEAP32.buffer, m + 4, r);
}

function createMesh(distance, height) {
    clearScene();

    const geometry = new THREE.BufferGeometry();

    geometry.setIndex(new THREE.BufferAttribute(indices, 1));
    geometry.setAttribute('position', new THREE.BufferAttribute(vertices, 3));

    const material = new THREE.MeshBasicMaterial({ color: 0xffffff });
    material.wireframe = true;
    
    const mesh = new THREE.Mesh(geometry, material);
    scene3D.add(mesh);

    camera.position.x = distance;
    camera.position.y = distance;
    camera.position.z = distance;
    camera.lookAt(0, height, 0);

    function animate() {
        requestAnimationFrame(animate);

        mesh.rotation.y += 0.01;

        renderer.render(scene3D, camera);
    };

    animate();
}