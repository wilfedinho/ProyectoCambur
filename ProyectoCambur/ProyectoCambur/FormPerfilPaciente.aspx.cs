using System;
using System.Collections.Generic;
using System.Data;

public partial class FormPerfilPaciente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarPacienteDemo();
            CargarPerfilesAnterioresDemo();
            MostrarEstado(1);
        }
    }
    private void MostrarEstado(int estado)
    {
        pnlSeleccion.Visible = (estado == 1);
        pnlResultado.Visible = (estado == 2);
        lblHeaderTitulo.Text = estado == 1
            ? "Generar perfil del paciente"
            : "Perfil generado";
    }
    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text = "LM";
    }
    private void CargarPacienteDemo()
    {
        lblPacienteIniciales.Text = "MG";
        lblPacienteNombre.Text = "Martín González";
        lblPacienteEdad.Text = "33 años";
        lblPacienteConsultas.Text = "12 consultas registradas";
    }
    private void CargarPerfilesAnterioresDemo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Modelo", typeof(string));
        dt.Columns.Add("Fecha", typeof(DateTime));

        dt.Rows.Add("Big Five (BFI)", new DateTime(2026, 3, 10));
        dt.Rows.Add("Estilos de Apego (ECR)", new DateTime(2026, 1, 20));

        if (dt.Rows.Count > 0)
        {
            rptPerfilesAnteriores.DataSource = dt;
            rptPerfilesAnteriores.DataBind();
            lblSinPerfiles.Visible = false;
        }
        else
        {
            rptPerfilesAnteriores.DataSource = null;
            rptPerfilesAnteriores.DataBind();
            lblSinPerfiles.Visible = true;
        }
    }
    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        string modelo = hfModeloSeleccionado.Value;

        if (string.IsNullOrEmpty(modelo))
        {
            MostrarError("Seleccioná un modelo de evaluación antes de generar el perfil.");
            return;
        }
        CargarPerfilDemo(modelo);
        MostrarEstado(2);
    }
    private void CargarPerfilDemo(string modelo)
    {
        string nombreModelo = string.Empty;
        string descripcion = string.Empty;
        string dimensiones = string.Empty;
        string patrones = string.Empty;
        string consideraciones = string.Empty;

        switch (modelo)
        {
            case "BIGFIVE":
                nombreModelo = "Big Five (BFI)";
                descripcion =
                    "El análisis de la información clínica disponible sugiere un perfil de personalidad " +
                    "caracterizado por niveles elevados de neuroticismo, moderada apertura a la experiencia " +
                    "y tendencia hacia la introversión. Se observa una orientación hacia el detalle y la " +
                    "responsabilidad en el ámbito laboral, contrastada con dificultades en la flexibilidad " +
                    "cognitiva ante situaciones de cambio o incertidumbre.";
                dimensiones =
                    "• Apertura a la experiencia: Moderada. Muestra curiosidad intelectual pero resistencia al cambio en contextos relacionales.\n" +
                    "• Responsabilidad: Alta. Se observa tendencia al perfeccionismo y alta exigencia personal.\n" +
                    "• Extroversión: Baja-moderada. Prefiere entornos predecibles y relaciones de confianza acotadas.\n" +
                    "• Amabilidad: Moderada. Cooperativo en superficie, con dificultades para expresar desacuerdo directamente.\n" +
                    "• Neuroticismo: Alto. Reactividad emocional elevada ante situaciones evaluativas y conflicto.";
                patrones =
                    "• Activación ansiosa recurrente ante situaciones de evaluación por figuras de autoridad.\n" +
                    "• Patrón de hipervigilancia sostenida en entornos laborales percibidos como hostiles.\n" +
                    "• Tendencia a la rumiación cognitiva post-conflicto con dificultad para el desenganche mental.\n" +
                    "• Inhibición de la expresión emocional directa como estrategia adaptativa aprendida.";
                consideraciones =
                    "El perfil de neuroticismo elevado sugiere la importancia de continuar fortaleciendo " +
                    "las estrategias de regulación emocional. El alto nivel de responsabilidad puede funcionar " +
                    "como recurso adaptativo si se trabaja la flexibilización de los estándares de autoexigencia. " +
                    "Se recomienda abordar la expresión emocional asertiva como objetivo terapéutico prioritario.";
                break;

            case "COPE":
                nombreModelo = "COPE Simplificado";
                descripcion =
                    "El análisis contextual de las estrategias de afrontamiento identificadas a lo largo del " +
                    "tratamiento muestra una predominancia de estilos evítativos ante situaciones de conflicto " +
                    "interpersonal, combinada con una mayor orientación hacia el afrontamiento activo en " +
                    "contextos laborales estructurados. Se observa escaso uso del soporte social como recurso.";
                dimensiones =
                    "• Afrontamiento activo: Moderado. Se activa principalmente en contextos con estructura clara.\n" +
                    "• Afrontamiento evitativo: Elevado. Predomina ante conflictos relacionales y situaciones de crítica.\n" +
                    "• Búsqueda de soporte social: Bajo. El paciente tiende a gestionar el malestar en soledad.\n" +
                    "• Reencuadre positivo: Moderado-bajo. Emerge en las últimas sesiones del período analizado.\n" +
                    "• Desconexión conductual: Moderada. Se observa en episodios de alta activación ansiosa.";
                patrones =
                    "• Tendencia a la evitación conductual ante situaciones que anticipan evaluación negativa.\n" +
                    "• Uso de la racionalización como mecanismo de distancia emocional ante el conflicto.\n" +
                    "• Resistencia a solicitar ayuda por temor a ser percibido como incompetente.\n" +
                    "• Mejora progresiva en el uso de técnicas de regulación emocional activa (últimas 4 sesiones).";
                consideraciones =
                    "Se recomienda continuar fortaleciendo el repertorio de estrategias de afrontamiento activo " +
                    "y trabajar la apertura al soporte social como recurso legítimo. El abordaje del temor al " +
                    "juicio externo resultará clave para reducir los patrones de evitación relacional.";
                break;

            case "AUTOEFICACIA":
                nombreModelo = "Autoeficacia de Schwarzer";
                descripcion =
                    "El análisis de la información clínica disponible evidencia un nivel de autoeficacia " +
                    "percibida moderado-bajo, con marcada variabilidad según el dominio de vida. En el " +
                    "ámbito laboral la percepción de competencia es más elevada cuando las tareas son " +
                    "autónomas, pero decrece significativamente en situaciones relacionales o de exposición " +
                    "al juicio de otros.";
                dimensiones =
                    "• Autoeficacia general: Moderada-baja. Fluctúa significativamente según contexto y expectativa de evaluación.\n" +
                    "• Autoeficacia social: Baja. Dificultades para confiar en sus propias capacidades en entornos relacionales.\n" +
                    "• Autoeficacia ante adversidad: Moderada. Muestra mayor resiliencia ante obstáculos externos que ante conflictos interpersonales.\n" +
                    "• Percepción de control: Moderada-baja. Tendencia a externalizar la responsabilidad de los resultados negativos.";
                patrones =
                    "• Discrepancia marcada entre competencias reales observadas y percepción subjetiva de las mismas.\n" +
                    "• Autoeficacia como variable modulada principalmente por el feedback de figuras de autoridad.\n" +
                    "• Patrón de profecía autocumplida: la baja expectativa de éxito reduce el compromiso conductual.\n" +
                    "• Incremento progresivo de la autoconfianza observable en las últimas sesiones del período.";
                consideraciones =
                    "El trabajo sobre las creencias nucleares de incompetencia resulta central para modificar " +
                    "la autoeficacia percibida de manera estructural. Se recomienda utilizar técnicas de " +
                    "activación conductual y registro de logros para construir evidencia contradictoria " +
                    "a las creencias limitantes actuales.";
                break;

            case "APEGO":
                nombreModelo = "Estilos de Apego Adulto (ECR)";
                descripcion =
                    "El análisis contextual sugiere un patrón de apego predominantemente ansioso, " +
                    "con activación marcada de conductas de hipervigilancia ante posibles señales de " +
                    "rechazo o abandono en vínculos significativos. Se observa una oscilación entre " +
                    "la búsqueda de proximidad emocional y la inhibición de las necesidades de apego " +
                    "por temor a la vulnerabilidad.";
                dimensiones =
                    "• Ansiedad de apego: Alta. Elevada preocupación por el abandono y la disponibilidad de figuras significativas.\n" +
                    "• Evitación de la intimidad: Moderada. Presente como mecanismo defensivo ante el temor a la dependencia.\n" +
                    "• Sensibilidad al rechazo: Alta. Interpretaciones sesgadas de señales neutras como indicadores de rechazo.\n" +
                    "• Base segura internalizada: Baja-moderada. En desarrollo progresivo a partir del vínculo terapéutico.";
                patrones =
                    "• Historia de apego inseguro con figura paterna ausente como antecedente relacional relevante.\n" +
                    "• Activación del sistema de apego ante conflictos con figuras de autoridad (patrón recurrente).\n" +
                    "• Oscilación entre la búsqueda de validación externa y la desconexión emocional como autoprotección.\n" +
                    "• El vínculo terapéutico ha comenzado a funcionar como experiencia correctiva de apego.";
                consideraciones =
                    "El trabajo sobre el estilo de apego ansioso requiere especial atención al vínculo " +
                    "terapéutico como espacio de experiencia correctiva. Se recomienda la exploración " +
                    "de la historia relacional temprana y el trabajo sobre la regulación emocional en " +
                    "contextos de vinculación íntima.";
                break;

            case "VALORES":
                nombreModelo = "Valores y Sentido de Vida (PVQ/Logoterapia)";
                descripcion =
                    "El análisis de la información clínica disponible revela una jerarquía de valores " +
                    "centrada en la seguridad, el logro y el reconocimiento social, con escasa conexión " +
                    "consciente con valores de trascendencia o sentido de vida. Se observa una disonancia " +
                    "entre los valores declarados (familia, autenticidad) y los valores actuados " +
                    "(rendimiento, aprobación externa).";
                dimensiones =
                    "• Valores de logro: Muy altos. Orientación marcada hacia el rendimiento y el reconocimiento.\n" +
                    "• Valores de seguridad: Altos. Necesidad de estructura, predictibilidad y estabilidad.\n" +
                    "• Valores de benevolencia: Moderados. Presentes en el discurso pero frecuentemente subordinados al logro.\n" +
                    "• Sentido de vida percibido: Moderado-bajo. Dificultades para identificar fuentes de significado más allá del rendimiento.\n" +
                    "• Autotrascendencia: Baja. Escasa conexión con propósitos que superen el interés personal inmediato.";
                patrones =
                    "• Construcción de identidad predominantemente anclada en el rendimiento laboral y la aprobación externa.\n" +
                    "• Vacío existencial encubierto bajo una elevada actividad productiva como mecanismo de evitación.\n" +
                    "• Disonancia entre el deseo de autenticidad relacional y la conducta de adaptación complaciente.\n" +
                    "• Emergencia progresiva de preguntas de sentido a partir de la 5ª sesión del período analizado.";
                consideraciones =
                    "Se recomienda explorar el sentido de vida como eje complementario al trabajo cognitivo, " +
                    "integrando técnicas de enfoque logoterapéutico. La clarificación de valores podría " +
                    "funcionar como ancla para la toma de decisiones más alineada con la autenticidad personal, " +
                    "reduciendo la dependencia de la validación externa como fuente de bienestar.";
                break;

            default:
                MostrarError("Modelo no reconocido. Seleccioná una opción válida.");
                return;
        }
        lblResultadoMeta.Text = "Paciente: Martín González · " + nombreModelo;
        lblModeloUsado.Text = "🧠 Modelo: " + nombreModelo;
        lblDescripcionGeneral.Text = descripcion;
        lblDimensiones.Text = dimensiones;
        lblPatrones.Text = patrones;
        lblConsideraciones.Text = consideraciones;
        lblMetaPaciente.Text = "Martín González";
        lblMetaModelo.Text = nombreModelo;
        lblMetaConsultas.Text = "12 consultas + historial clínico";
        lblMetaFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
    }
    protected void btnNuevoPerfil_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        hfModeloSeleccionado.Value = string.Empty;
        CargarPerfilesAnterioresDemo();
        MostrarEstado(1);
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        MostrarExito("Perfil guardado y encriptado correctamente. Disponible para exportar en PDF.");
    }
    private void MostrarError(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }

    private void MostrarExito(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-success";
        lblMensaje.Visible = true;
    }
}
