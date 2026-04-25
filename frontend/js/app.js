// Элементы страницы
const tasksList = document.getElementById("tasksList");
const loadingText = document.getElementById("loadingText");
const errorMessage = document.getElementById("errorMessage");
const inputTitle = document.getElementById("inputTitle");
const inputDesc = document.getElementById("inputDescription");
const btnAdd = document.getElementById("btnAdd");
const tasksCount = document.getElementById("tasksCount");
// Состояние
let allTasks = []; // все задачи с сервера
let activeFilter = "all"; // текущий фильтр
// Отрисовка задач
function renderTasks(tasks) {
  loadingText.style.display = "none";
  // Обновляем счётчик
  const total = allTasks.length;
  const completed = allTasks.filter((t) => t.isCompleted).length;
  tasksCount.textContent = `${completed} из ${total} выполнено`;
  if (tasks.length === 0) {
    tasksList.innerHTML = '<p class="empty-text">Задач не найдено</p>';
    return;
  }
  tasksList.innerHTML = tasks.map((task) => createTaskHTML(task)).join("");
}
function createTaskHTML(task) {
  const date = new Date(task.createdAt).toLocaleDateString("ru-RU");
  const desc = task.description ? `<p class="task-description">${escapeHtml(task.description)}</p>` : "";
  return `
<li class="task-item ${task.isCompleted ? "completed" : ""}"
id="task-${task.id}">
<!-- Режим просмотра -->
<div class="task-view">
<input type="checkbox"
class="task-checkbox"
${task.isCompleted ? "checked" : ""}
onchange="handleToggle(${task.id}, this.checked)" />
<div class="task-content">
<p class="task-title">${escapeHtml(task.title)}</p>
${desc}
<p class="task-date">Создано: ${date}</p>
</div>
<div class="task-actions">
<button class="btn-edit-task"
onclick="showEditMode(${task.id})">
✏️
</button>
<button class="btn-delete-task"
onclick="handleDelete(${task.id})">
🗑️
</button>
</div>
</div>
<!-- Режим редактирования -->
<div class="task-edit" id="edit-${task.id}">
<input type="text"
id="editTitle-${task.id}"
value="${escapeHtml(task.title)}" />
<textarea id="editDesc-${task.id}"
rows="2">${escapeHtml(task.description)}</textarea>
<p class="edit-error" id="editError-${task.id}"></p>
<div class="edit-buttons">
<button class="btn-save-task"
onclick="handleUpdate(${task.id}, ${task.isCompleted})">
💾 Сохранить
</button>
<button class="btn-cancel-task"
onclick="hideEditMode(${task.id})">
Отмена
</button>
</div>
</div>
</li>
`;
}
// Переключение режимов карточки
function showEditMode(id) {
  const item = document.getElementById(`task-${id}`);
  const viewMode = item.querySelector(".task-view");
  const editMode = document.getElementById(`edit-${id}`);
  viewMode.style.display = "none";
  editMode.style.display = "flex";
  // Фокус на поле заголовка
  document.getElementById(`editTitle-${id}`).focus();
}
function hideEditMode(id) {
  const item = document.getElementById(`task-${id}`);
  const viewMode = item.querySelector(".task-view");
  const editMode = document.getElementById(`edit-${id}`);
  viewMode.style.display = "flex";
  editMode.style.display = "none";
}
// Загрузка задач
async function loadTasks() {
  try {
    allTasks = await getAllTasks();
    applyFilter();
  } catch (error) {
    loadingText.style.display = "none";
    tasksList.innerHTML = `<p class="empty-text"> Ошибка: ❌ ${error.message}</p>`;
  }
}
// Фильтрация
function applyFilter() {
  let filtered = allTasks;
  if (activeFilter === "active") {
    filtered = allTasks.filter((t) => !t.isCompleted);
  } else if (activeFilter === "completed") {
    filtered = allTasks.filter((t) => t.isCompleted);
  }
  renderTasks(filtered);
}
// Добавление задачи
async function handleAdd() {
  const title = inputTitle.value.trim();
  const desc = inputDesc.value.trim();
  errorMessage.textContent = "";
  if (!title) {
    errorMessage.textContent = "Введите название задачи";
    inputTitle.focus();
    return;
  }
  btnAdd.disabled = true;
  btnAdd.textContent = "Добавляем...";
  try {
    await createTask(title, desc);
    inputTitle.value = "";
    inputDesc.value = "";
    await loadTasks();
  } catch (error) {
    errorMessage.textContent = error.message;
  } finally {
    btnAdd.disabled = false;
    btnAdd.textContent = "Добавить задачу";
  }
}
// Отметить выполненной / невыполненной
async function handleToggle(id, isCompleted) {
  // Находим задачу в локальном массиве
  const task = allTasks.find((t) => t.id === id);
  if (!task) return;
  try {
    await updateTask(id, task.title, task.description, isCompleted);
    await loadTasks();
  } catch (error) {
    alert("Ошибка: " + error.message);
    // Откатываем чекбокс обратно
    await loadTasks();
  }
}
// Редактирование задачи
async function handleUpdate(id, isCompleted) {
  const title = document.getElementById(`editTitle-${id}`).value.trim();
  const desc = document.getElementById(`editDesc-${id}`).value.trim();
  const errorEl = document.getElementById(`editError-${id}`);
  errorEl.textContent = "";
  if (!title) {
    errorEl.textContent = "Название не может быть пустым";
    return;
  }
  try {
    await updateTask(id, title, desc, isCompleted);
    await loadTasks();
  } catch (error) {
    errorEl.textContent = error.message;
  }
}
// Удаление задачи
async function handleDelete(id) {
  const task = allTasks.find((t) => t.id === id);
  const name = task ? `«${task.title}»` : `#${id}`;
  if (!confirm(`Удалить задачу ${name}?`)) return;
  try {
    await deleteTask(id);
    await loadTasks();
  } catch (error) {
    alert("Ошибка при удалении: " + error.message);
  }
}
// Обработчики событий
btnAdd.addEventListener("click", handleAdd);
inputTitle.addEventListener("keydown", function (e) {
  if (e.key === "Enter") handleAdd();
});
// Кнопки фильтров
document.querySelectorAll(".filter-btn").forEach((btn) => {
  btn.addEventListener("click", function () {
    // Убираем active у всех кнопок
    document.querySelectorAll(".filter-btn").forEach((b) => b.classList.remove("active"));
    // Добавляем active к нажатой
    this.classList.add("active");
    activeFilter = this.dataset.filter;
    applyFilter();
  });
});
// Вспомогательные функции
function escapeHtml(str) {
  return String(str ?? "")
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;");
}
// Запуск
loadTasks();